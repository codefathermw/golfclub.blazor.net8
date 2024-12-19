using GolfClub.BLL.Helpers;
using Microsoft.Extensions.Logging;

namespace GolfClub.BLL.Services.Files
{
    public class FileService(ILogger<FileService> logger) : IFileService
    {
        public async Task<BaseResponse<string>> SaveContentToFileAsync(string content)
        {
            try
            {
                var filePath = Path.Combine("wwwroot", "files", "paragraph.html");
                await File.WriteAllTextAsync(filePath, content);

                return BaseResponseFactory.IsOk<string>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an exception");

                return BaseResponseFactory.IsError<string>("An error occurred, failed to save contents to file");
            }
        }

        public async Task<BaseResponse<string>> LoadContentFromFileAsync()
        {
            try
            {
                var filePath = Path.Combine("wwwroot", "files", "paragraph.html");

                if (File.Exists(filePath))
                    return BaseResponseFactory.IsSuccess(await File.ReadAllTextAsync(filePath));

                return BaseResponseFactory.IsError<string>("<p>No content available.</p>");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Encountered an exception");
                return BaseResponseFactory.IsError<string>("An error occurred, failed to save contents to file");
            }
        }
    }
}
