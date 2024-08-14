using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.Models;
using Microsoft.Extensions.Logging;

namespace GolfClub.BLL.Services.Files
{
    public class FileService(ILogger<FileService> logger) : IFileService
    {
        private readonly ILogger<FileService> _logger = logger;

        public async Task<ResponseModel<string>> SaveContentToFileAsync(string content)
        {
            try
            {
                var filePath = Path.Combine("wwwroot", "files", "paragraph.html");
                await File.WriteAllTextAsync(filePath, content);

                return new ResponseModel<string>()
                {
                    IsErrorOccured = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an exception");

                return new ResponseModel<string>()
                {
                    IsErrorOccured = true,
                    Message = "An error occurred, failed to save contents to file"
                };
            }
        }

        public async Task<ResponseModel<string>> LoadContentFromFileAsync()
        {
            try
            {
                var filePath = Path.Combine("wwwroot", "files", "paragraph.html");

                if (File.Exists(filePath))
                {
                    return new ResponseModel<string>()
                    {
                        IsErrorOccured = false,
                        Result = await File.ReadAllTextAsync(filePath)
                    };
                }

                return new ResponseModel<string>()
                {
                    IsErrorOccured = false,
                    Result = "<p>No content available.</p>"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encountered an exception");

                return new ResponseModel<string>()
                {
                    IsErrorOccured = true,
                    Message = "An error occurred, failed to save contents to file"
                };
            }
        }
    }
}
