using GolfClub.BLL.DTOs;
using GolfClub.BLL.Models;
using GolfClub.DAL.Context;
using GolfClub.DAL.Models;
using GolfClub.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GolfClub.BLL.Services.Fittings
{
    public class FittingService(ILogger<FittingService> logger, IRepository<Fitting> fittingRepository) : IFittingService
    {
        private readonly ILogger _logger = logger;
        private readonly IRepository<Fitting> _fittingRepository = fittingRepository;

        static IEnumerable<TimeSpan> GetFreeSlots(List<TimeSpan> times, List<DateTime> dateTimes)
        {
            var timeSpans = dateTimes.Select(dt => dt.TimeOfDay);
            return times.Where(t => !timeSpans.Contains(t));
        }

        public async Task<ResponseModel<IEnumerable<TimeSpan>>> GetAvailableTimeSlot(DateTime date)
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

                var result = await _fittingRepository.GetAllAsync(u => u.ScheduledDate.Date.Equals(date.Date) && u.Status != "cancelled");

                if (result is not null)
                {
                    var freeSlots = GetFreeSlots(slots, result.Select(u => u.ScheduledDate).ToList());

                    return new ResponseModel<IEnumerable<TimeSpan>>
                    {
                        IsErrorOccured = false,
                        Result = freeSlots
                    };
                }

                return new ResponseModel<IEnumerable<TimeSpan>>
                {
                    IsErrorOccured = false,
                    Result = slots
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error while getting available time slots");

                return new ResponseModel<IEnumerable<TimeSpan>>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error while getting available time slots."
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<Fitting>>> GetFittingInProgress(int userId)
        {
            try
            {
                var result = await _fittingRepository.GetAllAsync(u => u.UserId == userId);

                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = false,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error");

                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error"
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<Fitting>>> GetFittingRequest()
        {
            try
            {
                var db = _fittingRepository.GetContext<AppDbContext>();
                var result = await db.FittingRequests.Include(u => u.User).ThenInclude(ur => ur.UserProfile).ToListAsync();
                
                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = false,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error");

                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error"
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<Fitting>>> GetMonthlyFittingsAsync(DateTime dateTime)
        {
            try
            {
                var result = await _fittingRepository.GetAllAsync(u => u.ScheduledDate.Month.Equals(dateTime.Month) && u.ScheduledDate.Year.Equals(dateTime.Year));

                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = false,
                    Result = result 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error enountered");

                return new ResponseModel<IEnumerable<Fitting>>
                {
                    IsErrorOccured = true,
                    Message = $"An error encountered"
                };
            }
        }

        public async Task<ResponseModel<bool>> SaveFittingRequest(SaveFittingRequestDto saveFittingRequestDto)
        {
            try
            {
                var entity = new Fitting()
                {
                    Comments = saveFittingRequestDto.Comments,
                    FittingType = saveFittingRequestDto.FittingType,
                    RequestDate = DateTime.Now,
                    ScheduledDate = saveFittingRequestDto.ScheduledDate,
                    Status = StatusConstants.Pending,
                    UserId = saveFittingRequestDto.UserId
                };

                await _fittingRepository.AddAsync(entity);
                await _fittingRepository.SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    IsErrorOccured = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error while saving {name} request", saveFittingRequestDto.FittingType);

                return new ResponseModel<bool>
                {
                    IsErrorOccured = true,
                    Message = $"Encountered an error while saving {saveFittingRequestDto.FittingType} request"
                };
            }
        }

        public async Task<ResponseModel<bool>> UpdateFittingStatus(int fittingId, string fittingStatus)
        {
            try
            {
                var result = await _fittingRepository.GetByIdAsync(fittingId);
                result.Status = fittingStatus;
                _fittingRepository.Update(result);
                await _fittingRepository.SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    IsErrorOccured = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an error while updating fitting status");

                return new ResponseModel<bool>
                {
                    IsErrorOccured = true,
                    Message = "Encountered an error while updating fitting status"
                };
            }
        }
    }
}
