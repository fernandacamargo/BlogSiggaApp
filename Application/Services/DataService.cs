using BlogSiggaApp.Application.Interfaces;

namespace BlogSiggaApp.Application.Services
{
    public class DataService
    {
        private readonly IFileStreamService _fileStreamService;


        public DataService(IFileStreamService fileStreamService)
        {
            _fileStreamService = fileStreamService;
        }

        public async Task ProcessDataAsync()
        {

            using (var stream = await _fileStreamService.GetFileStreamAsync())
            {
                
                using (var reader = new StreamReader(stream))
                {
                    string content = await reader.ReadToEndAsync();
                    Console.WriteLine(content); 
                }
            }

        }

    }
}
