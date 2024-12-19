namespace GolfClub.BLL.Helpers
{
    public static class BaseResponseFactory
    {
        public static BaseResponse<T> IsError<T>(string message = "An error occurred", T? result = default)
            => new() { IsErrorOccurred = true, Message = message, Result = result! };

        public static BaseResponse<T> IsSuccess<T>(T result, string message = "Success")
            => new() { IsErrorOccurred = false, Message = message, Result = result };

        public static BaseResponse<T> IsOk<T>() => new();

        public static BaseResponse<T> Respond<T>(bool isError, string message, T? result = default)
            => new() { IsErrorOccurred = isError, Message = message, Result = result! };
    }
}
