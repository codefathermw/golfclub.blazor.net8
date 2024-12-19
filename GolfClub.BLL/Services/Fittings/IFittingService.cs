using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Helpers;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.Fittings
{
    public interface IFittingService
    {
        Task<BaseResponse<IEnumerable<TimeSpan>>> GetAvailableTimeSlot(DateTime date);
        Task<BaseResponse<IEnumerable<Fitting>>> GetFittingRequest();
        Task<BaseResponse<bool>> UpdateFittingStatus(int fittingId, string fittingStatus);
        Task<BaseResponse<bool>> SaveFittingRequest(SaveFittingRequestDto saveFittingRequestDto);
        Task<BaseResponse<IEnumerable<Fitting>>> GetFittingInProgress(int userId);
        Task<BaseResponse<IEnumerable<Fitting>>> GetMonthlyFittingsAsync(DateTime dateTime);
    }
}
