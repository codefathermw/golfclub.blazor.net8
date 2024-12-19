namespace GolfClub.BLL.DTOs
{
    public class SaveFittingRequestDto
    {
        public DateTime ScheduledDate { get; set; }
        public string? Comments { get; set; }
        public required string FittingType { get; set; }
        public required int UserId { get; set; }
    }
}
