using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Models;

namespace GolfClub.BLL.Services.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<ResponseModel<ClaimsIdentity>> LoginAsync(LoginDto loginDto);
        Task<ResponseModel<string>> CreateAccountAsync(UserAccountDto signupDto);
        Task<ResponseModel<bool>> DoesUserNameExistsAsync(string userName);
        Task<ResponseModel<int>> GetUserId();
    }
}
