using GolfClub.BLL.DTOs;
using GolfClub.BLL.Helpers;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.Users
{
    public interface IUserService
    {
        Task<BaseResponse<UserProfile>> GetUserProfileAsync(int userId);
        Task<BaseResponse<string>> UpdateProfileAsync(UserProfile profile);
        Task<BaseResponse<string>> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto);
        Task<BaseResponse<List<UserAccount>>> GetUserProfilesAsync();
    }
}
