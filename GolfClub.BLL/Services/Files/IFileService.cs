using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.Helpers;

namespace GolfClub.BLL.Services.Files
{
    public interface IFileService
    {
        Task<BaseResponse<string>> SaveContentToFileAsync(string content);
        Task<BaseResponse<string>> LoadContentFromFileAsync();
    }
}
