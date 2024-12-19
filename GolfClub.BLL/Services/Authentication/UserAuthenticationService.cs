using System.Security.Claims;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Helpers;
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
    public class UserAuthenticationService(
        ILogger<UserAuthenticationService> logger, 
        IRepository<UserAccount> userRepository, 
        IRepository<UserProfile> userProfileRepository, 
        AuthenticationStateProvider AuthenticationStateProvider) : IUserAuthenticationService
    {
        public async Task<BaseResponse<int>> GetUserId()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                if (authState.User.Identity!.IsAuthenticated)
                {
                    var userId = authState.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    return BaseResponseFactory.IsSuccess(Convert.ToInt32(userId));
                }

                return BaseResponseFactory.IsError<int>("UserId not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error");
                return BaseResponseFactory.IsError<int>("An error occurred failed to get user id");
            }
        }

        public async Task<BaseResponse<string>> CreateAccountAsync(UserAccountDto signupDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signupDto.UserName) || string.IsNullOrWhiteSpace(signupDto.Password))
                    return BaseResponseFactory.IsError<string>("Username and password are required.");

                var existingUser = await userRepository.TryGetAsync(u => u.UserName == signupDto.UserName);

                if (existingUser is not null)
                    return BaseResponseFactory.IsError<string>("Username already exists.");

                var hashedPassword = PasswordService.HashPassword(signupDto.Password);

                var user = new UserAccount()
                {
                    UserName = signupDto.UserName,
                    PasswordHash = hashedPassword,
                    UserRoles = [ new () { RoleId = 2}]
                };

                var newUser = await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();

                var newProfile = new UserProfile
                {
                    UserId = newUser.UserId,
                    Email = signupDto.Email,
                    FirstName = signupDto.FirstName,
                    LastName = signupDto.LastName,
                };

                await userProfileRepository.AddAsync(newProfile);
                await userProfileRepository.SaveChangesAsync();

                return BaseResponseFactory.IsOk<string>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating account for username {username}", signupDto.UserName);
                return BaseResponseFactory.IsError<string>("An error occurred while creating the account.");
            }
        }

        public async Task<BaseResponse<bool>> DoesUserNameExistsAsync(string userName)
        {
            try
            {
                var user = await userRepository.TryGetAsync(u => u.UserName == userName);
                return BaseResponseFactory.IsSuccess(user is not null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if username {username} exists", userName);
                return BaseResponseFactory.IsError<bool>("An error occurred while checking the username.");
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var db = userRepository.GetContext<AppDbContext>();
                var user = await db.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

                if (user is null || !PasswordService.VerifyPassword(user.PasswordHash, loginDto.Password))
                    return BaseResponseFactory.IsError<ClaimsIdentity>("Incorrect credentials provided");

                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName),
                    new ("UserId", user.UserId.ToString())
                };

                if (user.UserRoles.Count == 0)
                    return BaseResponseFactory.IsError<ClaimsIdentity>("Failed to identify user roles");

                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new (ClaimTypes.Role, userRole.Role.RoleName));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return BaseResponseFactory.IsSuccess(claimsIdentity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred during login with username {Username}", loginDto.Username);
                return BaseResponseFactory.IsError<ClaimsIdentity>("An error occurred, failed to login.");
            }
        }

        public async Task<BaseResponse<bool>> DoesEmailExistsAsync(string email)
        {
            try
            {
                var user = await userProfileRepository.TryGetAsync(u => u.Email == email);
                return BaseResponseFactory.IsSuccess(user is not null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if email {email} exists", email);
                return BaseResponseFactory.IsError<bool>("An error occurred while checking the email.");
            }
        }
    }
}
