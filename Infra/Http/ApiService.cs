using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Infra.Http
{
    public class ApiService : IApiService
    {
        private readonly IWebService _webService;


        public ApiService(IWebService webService)
        {
            _webService = webService;
        }

        public async Task<List<Post>> FetchPostsAsync()
        {
            try
            {

                var response = await _webService.GetDataAsync();
                return response;
            }
            catch (HttpRequestException httpEx)
            {

                Console.WriteLine($"Erro ao fazer a requisição HTTP: {httpEx.Message}");
                return new List<Post>();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return new List<Post>();
            }
        }
    }
}
