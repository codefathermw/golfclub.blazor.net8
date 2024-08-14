using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolfClub.BLL.Models;

namespace GolfClub.BLL.Services.Files
{
    public interface IFileService
    {
        Task<ResponseModel<string>> SaveContentToFileAsync(string content);
        Task<ResponseModel<string>> LoadContentFromFileAsync();
    }
}
