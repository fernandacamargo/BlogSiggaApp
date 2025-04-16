using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Application.Services;
using BlogSiggaApp.Domain.Entities;
using System.Text.Json;

public class WebServiceRepository : BaseService, IWebServiceRepository
{
    private readonly HttpClient _httpClient;

    public WebServiceRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Post>> GetDataAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("posts");

            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseAsync(response);
            }

            var errorMessage = $"Erro ao obter dados. Status Code: {response.StatusCode}, Detalhes: {await response.Content.ReadAsStringAsync()}";
            LogError(errorMessage);
            return new List<Post>();
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
        {
            LogError($"Falha ao realizar a requisição HTTP. {ex}");
            return new List<Post>();
        }
        catch (Exception ex)
        {
            LogError($"Erro inesperado ao obter dados. {ex}");
            return new List<Post>();
        }
    }

    private async Task<List<Post>> DeserializeResponseAsync(HttpResponseMessage response)
    {
        using (Stream stream = await response.Content.ReadAsStreamAsync())
        {
            try
            {
                var posts = await JsonSerializer.DeserializeAsync<List<Post>>(stream);
                return posts ?? new List<Post>();
            }
            catch (JsonException ex)
            {
                LogError($"Falha ao deserializar resposta: {ex}");
                return new List<Post>();
            }
        }
    }

}
