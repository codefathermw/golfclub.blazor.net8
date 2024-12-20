using GolfClub.BLL.DTOs;
using GolfClub.BLL.Enums;
using GolfClub.BLL.Helpers;
using GolfClub.DAL.Models;

namespace GolfClub.BLL.Services.Fittings
{
    public interface IFittingService
    {
        Task<BaseResponse<IEnumerable<TimeSpan>>> GetAvailableTimeSlotAsync(DateTime date);
        Task<BaseResponse<IEnumerable<Fitting>>> GetFittingRequestAsync();
        Task<BaseResponse<Fitting>> GetFittingByReferenceNumberAsync(string referenceNumber);
        Task<BaseResponse<bool>> UpdateFittingStatusAsync(int fittingId, StatusEnum status);
        Task<BaseResponse<bool>> SaveFittingRequestAsync(SaveFittingRequestDto saveFittingRequestDto);
        Task<BaseResponse<IEnumerable<Fitting>>> GetFittingInProgressAsync(int userId);
        Task<BaseResponse<IEnumerable<Fitting>>> GetMonthlyFittingsAsync(DateTime dateTime);
    }
}
