using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.DTOs;
using GolfClub.BLL.Models;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.Fittings
{
    public interface IFittingService
    {
        Task<ResponseModel<IEnumerable<TimeSpan>>> GetAvailableTimeSlot(DateTime date);
        Task<ResponseModel<IEnumerable<Fitting>>> GetFittingRequest();
        Task<ResponseModel<bool>> UpdateFittingStatus(int fittingId, string fittingStatus);
        Task<ResponseModel<bool>> SaveFittingRequest(SaveFittingRequestDto saveFittingRequestDto);
        Task<ResponseModel<IEnumerable<Fitting>>> GetFittingInProgress(int userId);
        Task<ResponseModel<IEnumerable<Fitting>>> GetMonthlyFittingsAsync(DateTime dateTime);
    }
}
