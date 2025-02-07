using System;
using Microsoft.AspNetCore.Http;


namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string uploadDirectory);
        void DeleteFile(string filePath);
    }
}
