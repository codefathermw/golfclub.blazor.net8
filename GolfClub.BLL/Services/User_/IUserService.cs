using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Models;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.User_
{
    public interface IUserService
    {
        Task<ResponseModel<UserProfile>> GetUserProfile(int userId);
        Task<ResponseModel<string>> UpdateProfile(UserProfile profile);
        Task<ResponseModel<string>> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<ResponseModel<List<UserAccount>>> GetUserProfiles();
    }
}
