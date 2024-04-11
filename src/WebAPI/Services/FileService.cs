using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class FileService : IFileService
    {
        IWebHostEnvironment _appEnvironment;
        private const string fileFolder = "/Files/";
        public FileService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public async Task<byte[]> ReadFileBytes(string filePath)
        {
            string path = _appEnvironment.WebRootPath + fileFolder + filePath;
            var bytes = await File.ReadAllBytesAsync(path);

            return bytes;
        }

        public async Task SaveFile(IFormFile file, string filePath)
        {
            string path = fileFolder + filePath;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        void IFileService.DeleteFile(string filePath)
        {
            File.Delete(_appEnvironment.WebRootPath + fileFolder + filePath);
        }
    }
}
