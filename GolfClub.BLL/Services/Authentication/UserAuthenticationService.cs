using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Models;
using GolfClub.BLL.Services.Password;
using GolfClub.DAL.Context;
using GolfClub.DAL.Models;
using GolfClub.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GolfClub.BLL.Services.Authentication
{
    public class UserAuthenticationService(ILogger<UserAuthenticationService> logger, IRepository<UserAccount> userRepository, 
        IRepository<UserProfile> userProfileRepository, AuthenticationStateProvider AuthenticationStateProvider) : IUserAuthenticationService
    {
        public ILogger<UserAuthenticationService> _logger = logger;
        public IRepository<UserAccount> _userRepository = userRepository;
        public IRepository<UserProfile> _userProfileRepository = userProfileRepository;

        public async Task<ResponseModel<int>> GetUserId()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                if (authState.User.Identity.IsAuthenticated)
                {
                    var userId = authState.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    return new ResponseModel<int>
                    {
                        IsErrorOccured = false,
                        Result = Convert.ToInt32(userId)
                    };
                }

                return new ResponseModel<int>
                {
                    IsErrorOccured = true,
                    Message = "UserId not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error");

                return new ResponseModel<int>
                {
                    IsErrorOccured = true,
                    Message = "An error occurred failed to get user id"
                };
            }
        }

        public async Task<ResponseModel<string>> CreateAccountAsync(UserAccountDto signupDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signupDto.UserName) || string.IsNullOrWhiteSpace(signupDto.Password))
                {
                    return new ResponseModel<string>
                    {
                        IsErrorOccured = true,
                        Message = "Username and password are required."
                    };
                }

                var existingUser = await _userRepository.GetAsync(u => u.UserName == signupDto.UserName);

                if (existingUser is not null)
                {
                    return new ResponseModel<string>
                    {
                        IsErrorOccured = true,
                        Message = "Username already exists."
                    };
                }

                var hashedPassword = PasswordService.HashPassword(signupDto.Password);

                var user = new UserAccount()
                {
                    UserName = signupDto.UserName,
                    PasswordHash = hashedPassword,
                    UserRoles = [ new () { RoleId = 2}]
                };

                var newUser = await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                var newProfile = new UserProfile
                {
                    UserId = newUser.UserId,
                    Email = signupDto.Email,
                    FirstName = signupDto.FirstName,
                    LastName = signupDto.LastName,
                };

                await _userProfileRepository.AddAsync(newProfile);
                await _userProfileRepository.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    IsErrorOccured = false,
                    Result = "Account created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account for username {username}", signupDto.UserName);

                return new ResponseModel<string>
                {
                    IsErrorOccured = true,
                    Message = "An error occurred while creating the account."
                };
            }
        }

        public async Task<ResponseModel<bool>> DoesUserNameExistsAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.UserName == userName);

                return new ResponseModel<bool>
                {
                    IsErrorOccured = false,
                    Result = user is not null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if username {username} exists", userName);

                return new ResponseModel<bool>
                {
                    IsErrorOccured = true,
                    Message = "An error occurred while checking the username."
                };
            }
        }

        public async Task<ResponseModel<ClaimsIdentity>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var db = _userRepository.GetContext<AppDbContext>();
                var user = await db.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

                if (user is null || !PasswordService.VerifyPassword(user.PasswordHash, loginDto.Password))
                {
                    return new ResponseModel<ClaimsIdentity>
                    {
                        IsErrorOccured = true,
                        Message = "Incorrect credentials provided"
                    };
                }

                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName),
                    new ("UserId", user.UserId.ToString())
                };

                if (user.UserRoles.Count == 0)
                {
                    return new ResponseModel<ClaimsIdentity>
                    {
                        IsErrorOccured = true,
                        Message = "Failed to identify user roles"
                    };
                }

                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new (ClaimTypes.Role, userRole.Role.RoleName));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return new ResponseModel<ClaimsIdentity>
                {
                    IsErrorOccured = false,
                    Result = claimsIdentity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during login with username {Username}", loginDto.Username);

                return new ResponseModel<ClaimsIdentity>
                {
                    IsErrorOccured = true,
                    Message = "An error occurred, failed to login."
                };
            }
        }
    }
}
