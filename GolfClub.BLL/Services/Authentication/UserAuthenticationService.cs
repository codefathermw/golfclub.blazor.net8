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
                    return BaseResponseFactory.Success(Convert.ToInt32(userId));
                }

                return BaseResponseFactory.Error<int>("UserId not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error");
                return BaseResponseFactory.Error<int>("Failed to get user id");
            }
        }

        public async Task<BaseResponse<string>> CreateAccountAsync(UserDto signupDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(signupDto.UserName) || string.IsNullOrWhiteSpace(signupDto.Password))
                    return BaseResponseFactory.Error<string>("Username and password are required.");

                var existingUser = await userRepository.TryGetAsync(u => u.UserName == signupDto.UserName);

                if (existingUser is not null)
                    return BaseResponseFactory.Error<string>("Username already exists.");

                var hashedPassword = PasswordService.HashPassword(signupDto.Password);
                var user = new UserAccount()
                {
                    UserName = signupDto.UserName,
                    PasswordHash = hashedPassword,
                    UserRoles = [ new () { RoleId = 2}],
                    DateCreated = DateTime.Now,
                };

                var newUser = await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();
                var newProfile = new UserProfile
                {
                    UserId = newUser.UserId,
                    Email = signupDto.Email,
                    FirstName = signupDto.FirstName,
                    LastName = signupDto.LastName,
                    DateUpdated = DateTime.Now
                };

                await userProfileRepository.AddAsync(newProfile);
                await userProfileRepository.SaveChangesAsync();
                return BaseResponseFactory.Ok<string>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating account for username {username}", signupDto.UserName);
                return BaseResponseFactory.Error<string>("Failed to create account.");
            }
        }

        public async Task<BaseResponse<bool>> DoesUserNameExistsAsync(string userName)
        {
            try
            {
                var user = await userRepository.TryGetAsync(u => u.UserName == userName);
                return BaseResponseFactory.Success(user is not null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if username {username} exists", userName);
                return BaseResponseFactory.Error<bool>("Failed to check username.");
            }
        }

        //public async Task<BaseResponse<string>> VerifyIdentity(int userId, string password)
        //{
        //    try
        //    {
        //        var db = userRepository.GetContext<AppDbContext>();
        //        var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        //        if (user is null || !PasswordService.VerifyPassword(user.PasswordHash, password))
        //            return BaseResponseFactory.Error<string>("Incorrect credentials provided");

        //        return BaseResponseFactory.Success<string>(user.UserName);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Exception occurred trying to verify identity");
        //        return BaseResponseFactory.Error<string>("Failed to login.");
        //    }
        //}

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
                    return BaseResponseFactory.Error<ClaimsIdentity>("Incorrect credentials provided");

                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName),
                    new ("UserId", user.UserId.ToString())
                };

                if (user.UserRoles.Count == 0)
                    return BaseResponseFactory.Error<ClaimsIdentity>("Failed to identify user roles");

                foreach (var userRole in user.UserRoles)
                {
                    claims.Add(new (ClaimTypes.Role, userRole.Role.RoleName));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return BaseResponseFactory.Success(claimsIdentity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred during login with username {Username}", loginDto.Username);
                return BaseResponseFactory.Error<ClaimsIdentity>("Failed to login.");
            }
        }

        public async Task<BaseResponse<bool>> DoesEmailExistsAsync(string email)
        {
            try
            {
                var user = await userProfileRepository.TryGetAsync(u => u.Email == email);
                return BaseResponseFactory.Success(user is not null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if email {email} exists", email);
                return BaseResponseFactory.Error<bool>("Failed to check email.");
            }
        }
    }
}
