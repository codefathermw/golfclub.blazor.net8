using GolfClub.BLL.DTOs;
using GolfClub.BLL.Helpers;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.Users
{
    public interface IUserService
    {
        Task<BaseResponse<UserProfile>> GetUserProfile(int userId);
        Task<BaseResponse<string>> UpdateProfile(UserProfile profile);
        Task<BaseResponse<string>> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<BaseResponse<List<UserAccount>>> GetUserProfiles();
    }
}
