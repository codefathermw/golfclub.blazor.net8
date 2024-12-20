using GolfClub.BLL.DTOs;
using GolfClub.BLL.Enums;
using GolfClub.BLL.Helpers;
using GolfClub.DAL.Context;
using GolfClub.DAL.Models;
using GolfClub.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GolfClub.BLL.Services.Fittings
{
    public class FittingService(
        ILogger<FittingService> logger, 
        IRepository<Fitting> fittingRepository) : IFittingService
    {
        static IEnumerable<TimeSpan> GetFreeSlots(List<TimeSpan> times, List<DateTime> dateTimes)
        {
            var timeSpans = dateTimes.Select(dt => dt.TimeOfDay);
            return times.Where(t => !timeSpans.Contains(t));
        }

        public async Task<BaseResponse<IEnumerable<TimeSpan>>> GetAvailableTimeSlotAsync(DateTime date)
        {
            try
            {
                var slots = new List<TimeSpan>
                {
                    new (9, 0, 0),
                    new (10, 0, 0),
                    new (11, 0, 0),
                    new (13, 0, 0),
                    new (14, 0, 0),
                    new (15, 0, 0)
                };

                var result = await fittingRepository.GetAllAsync(u => u.ScheduledDate.Date.Equals(date.Date) && u.Status != "cancelled");

                if (result is not null)
                {
                    var freeSlots = GetFreeSlots(slots, result.Select(u => u.ScheduledDate).ToList());

                    return BaseResponseFactory.IsSuccess(freeSlots);
                }

                return BaseResponseFactory.IsSuccess<IEnumerable<TimeSpan>>(slots);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error while getting available time slots");

                return BaseResponseFactory.IsError<IEnumerable<TimeSpan>>("Encountered an error while getting available time slots.");
            }
        }

        public async Task<BaseResponse<Fitting>> GetFittingByReferenceNumberAsync(string referenceNumber)
        {
            try
            {
                var db = fittingRepository.GetContext<AppDbContext>();
                var result = await db!.FittingRequests
                    .Where(u => u.ReferenceNumber == referenceNumber)
                    .Include(u => u.User)
                    .ThenInclude(ur => ur.UserProfile)
                    .FirstOrDefaultAsync();

                if (result is null)
                    return BaseResponseFactory.IsError<Fitting>("Not found");

                return BaseResponseFactory.IsSuccess(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error");

                return BaseResponseFactory.IsError<Fitting>();
            }
        }

        public async Task<BaseResponse<IEnumerable<Fitting>>> GetFittingInProgressAsync(int userId)
        {
            try
            {
                var result = await fittingRepository.GetAllAsync(u => u.UserId == userId);

                return BaseResponseFactory.IsSuccess(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error");

                return BaseResponseFactory.IsError<IEnumerable<Fitting>>();
            }
        }

        public async Task<BaseResponse<IEnumerable<Fitting>>> GetFittingRequestAsync()
        {
            try
            {
                var db = fittingRepository.GetContext<AppDbContext>();
                var result = await db!.FittingRequests
                    .Include(u => u.User)
                    .ThenInclude(ur => ur.UserProfile)
                    .ToListAsync();
                
                return BaseResponseFactory.IsSuccess<IEnumerable<Fitting>>(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error");

                return BaseResponseFactory.IsError<IEnumerable<Fitting>>();
            }
        }

        public async Task<BaseResponse<IEnumerable<Fitting>>> GetMonthlyFittingsAsync(DateTime dateTime)
        {
            try
            {
                var result = await fittingRepository.GetAllAsync(u => u.ScheduledDate.Month.Equals(dateTime.Month) && u.ScheduledDate.Year.Equals(dateTime.Year));

                return BaseResponseFactory.IsSuccess(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error encountered");

                return BaseResponseFactory.IsError<IEnumerable<Fitting>>();
            }
        }

        public async Task<BaseResponse<bool>> SaveFittingRequestAsync(SaveFittingRequestDto saveFittingRequestDto)
        {
            try
            {
                var response = await GetMonthlyFittingsAsync(DateTime.Now);
                var refNum = $"F{DateTime.Now:yyMMdd}-{response.Result.Count()}" ;

                var entity = new Fitting()
                {
                    Comments = saveFittingRequestDto.Comments,
                    FittingType = saveFittingRequestDto.FittingType,
                    RequestDate = DateTime.Now,
                    ScheduledDate = saveFittingRequestDto.ScheduledDate,
                    Status = StatusEnum.Pending.ToString(),
                    UserId = saveFittingRequestDto.UserId,
                    ReferenceNumber = refNum
                };

                await fittingRepository.AddAsync(entity);
                await fittingRepository.SaveChangesAsync();

                return BaseResponseFactory.IsOk<bool>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error while saving {name} request", saveFittingRequestDto.FittingType);

                return BaseResponseFactory.IsError<bool>($"Encountered an error while saving {saveFittingRequestDto.FittingType} request");
            }
        }

        public async Task<BaseResponse<bool>> UpdateFittingStatusAsync(int fittingId, StatusEnum status)
        {
            try
            {
                var result = await fittingRepository.TryGetByIdAsync(fittingId);
                result!.Status = status.ToString();
                fittingRepository.Update(result);
                await fittingRepository.SaveChangesAsync();

                return BaseResponseFactory.IsOk<bool>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an error while updating fitting status");

                return BaseResponseFactory.IsError<bool>("Encountered an error while updating fitting status");
            }
        }
    }
}
