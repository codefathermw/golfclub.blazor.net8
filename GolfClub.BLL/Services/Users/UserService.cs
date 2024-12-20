using Microsoft.Extensions.Logging;
using GolfClub.DAL.Repositories;
using GolfClub.DAL.Models;
using GolfClub.BLL.DTOs;
using GolfClub.DAL.Context;
using Microsoft.EntityFrameworkCore;
using GolfClub.BLL.Services.Password;
using GolfClub.BLL.Helpers;

namespace GolfClub.BLL.Services.Users
{
    public class UserService(
        ILogger<UserService> logger,
        IRepository<UserAccount> userRepository,
        IRepository<UserProfile> profileRepository) : IUserService
    {
        public async Task<BaseResponse<string>> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var db = userRepository.GetContext<AppDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == updatePasswordDto.UserName);

                if (user is null || !PasswordService.VerifyPassword(updatePasswordDto.OldPassword, user.PasswordHash))
                    return BaseResponseFactory.IsError<string>("Invalid old password.");

                user.PasswordHash = PasswordService.HashPassword(updatePasswordDto.NewPassword);
                userRepository.Update(user);
                await db.SaveChangesAsync();

                return BaseResponseFactory.IsOk<string>();
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Encountered an error");

                return BaseResponseFactory.IsError<string>("Encountered an error");
            }
        }

        public async Task<BaseResponse<UserProfile>> GetUserProfileAsync(int userId)
        {
            try
            {
                var profile = await profileRepository.TryGetByIdAsync(userId);

                return BaseResponseFactory.IsSuccess(profile!);
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Encountered an error");

                return BaseResponseFactory.IsError<UserProfile>("Encountered an error");
            }
        }

        public async Task<BaseResponse<string>> UpdateProfileAsync(UserProfile profile)
        {
            try
            {
                profile.DateUpdated = DateTime.Now;
                profileRepository.Update(profile);
                await profileRepository.SaveChangesAsync();

                return BaseResponseFactory.IsOk<string>();
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Encountered an error");

                return BaseResponseFactory.IsError<string>("Encountered an error");
            }
        }

        public async Task<BaseResponse<List<UserAccount>>> GetUserProfilesAsync()
        {
            try
            {
                var db = userRepository.GetContext<AppDbContext>();
                var users = await db.Users
                    .Where(u => u.UserId != 1)
                    .Include(q => q.UserProfile)
                    .ToListAsync();

                return BaseResponseFactory.IsSuccess(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred");

                return BaseResponseFactory.IsError<List<UserAccount>>("An error occurred");
            }
        }
    }
}
