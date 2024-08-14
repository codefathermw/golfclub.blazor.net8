using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.Models;
using GolfClub.BLL.Services.Password;
using Microsoft.Extensions.Logging;
using GolfClub.DAL.Repositories;
using GolfClub.DAL.Models;
using GolfClub.BLL.DTOs;
using GolfClub.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.BLL.Services.User_
{
    public class UserService(ILogger<UserService> logger, IRepository<UserAccount> userRepository, IRepository<UserProfile> profileRepository) 
        : IUserService
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly IRepository<UserAccount> _userRepository = userRepository;
        private readonly IRepository<UserProfile> _profileRepository = profileRepository;

        public async Task<ResponseModel<string>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var db = _userRepository.GetContext<AppDbContext>();
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == changePasswordDto.UserName);

                if (user is null || !PasswordService.VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash))
                {
                    return new ResponseModel<string>
                    {
                        IsErrorOccured = true,
                        Message = "Invalid old password."
                    };
                }

                user.PasswordHash = PasswordService.HashPassword(changePasswordDto.NewPassword);
                _userRepository.Update(user);
                await db.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    IsErrorOccured = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Encountered an error");

                return new ResponseModel<string>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error"
                };
            }
        }

        public async Task<ResponseModel<UserProfile>> GetUserProfile(int userId)
        {
            try
            {
                var profile = await _profileRepository.GetByIdAsync(userId);

                return new ResponseModel<UserProfile>
                {
                    IsErrorOccured = false,
                    Result = profile
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Encountered an error");

                return new ResponseModel<UserProfile>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error"
                };
            }
        }

        public async Task<ResponseModel<string>> UpdateProfile(UserProfile profile)
        {
            try
            {
                _profileRepository.Update(profile);
                await _profileRepository.SaveChangesAsync();

                return new ResponseModel<string>
                {
                    IsErrorOccured = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Encountered an error");

                return new ResponseModel<string>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error"
                };
            }
        }
    
        public async Task<ResponseModel<List<UserAccount>>> GetUserProfiles()
        {
            try
            {
                var db = _userRepository.GetContext<AppDbContext>();
                var users = await db.Users.Where(u => u.UserId == 1).Include(q => q.UserProfile).ToListAsync();

                return new ResponseModel<List<UserAccount>>()
                {
                    IsErrorOccured = false,
                    Result = users
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");

                return new ResponseModel<List<UserAccount>>()
                {
                    IsErrorOccured = true,
                    Message = "An error occured"
                };
            }
        }
    
    }
}
