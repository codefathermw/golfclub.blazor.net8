namespace GolfClub.BLL.Helpers
{
    public class BaseResponse<T>
    {
        public bool IsErrorOccurred { get; set; }
        public string Message { get; set; } = null!;
        public T Result { get; set; } = default!;
    }
}
