using System.Security.Claims;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Helpers;

namespace GolfClub.BLL.Services.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<BaseResponse<ClaimsIdentity>> LoginAsync(LoginDto loginDto);
        Task<BaseResponse<string>> CreateAccountAsync(UserAccountDto signupDto);
        Task<BaseResponse<bool>> DoesUserNameExistsAsync(string userName);
        Task<BaseResponse<bool>> DoesEmailExistsAsync(string userName);
        Task<BaseResponse<int>> GetUserId();
    }
}
