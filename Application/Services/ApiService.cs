using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly IWebServiceRepository _webService;


        public ApiService(IWebServiceRepository webService)
        {
            _webService = webService;
        }

        public async Task<List<Post>> FetchPostsAsync()
        {
            return await _webService.GetDataAsync();
        
        }
    }
}
