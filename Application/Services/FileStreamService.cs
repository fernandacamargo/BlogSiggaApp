using BlogSiggaApp.Application.Interfaces;

namespace BlogSiggaApp.Application.Services
{
    public class FileStreamService : IFileStreamService
    {
        private readonly string _filePath;

        public FileStreamService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<Stream> GetFileStreamAsync()
        {

            if (!File.Exists(_filePath))
                throw new FileNotFoundException($"O arquivo '{_filePath}' não foi encontrado.");

    
            return await Task.FromResult(new FileStream(_filePath, FileMode.Open, FileAccess.Read));
        }
    }

}
