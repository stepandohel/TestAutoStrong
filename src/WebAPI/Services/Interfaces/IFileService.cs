namespace WebAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task SaveFile(IFormFile file, string filePath);
        Task<byte[]> ReadFileBytes(string filePath);
        void DeleteFile(string filePath);
    }
}
