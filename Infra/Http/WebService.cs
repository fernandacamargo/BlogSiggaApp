using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;
using System.Text.Json;

public class WebService : IWebService
{
    private readonly HttpClient _httpClient;

    // O HttpClient é injetado com a URL base configurada
    public WebService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Post>> GetDataAsync()
    {
        try
        {
           
            var response = await _httpClient.GetAsync(""); 

            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                  
                    var posts = await JsonSerializer.DeserializeAsync<List<Post>>(stream);
                    return posts;
                }
            }
            else
            {
                throw new Exception($"Erro ao obter dados da API. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
      
            throw new Exception($"Falha na requisição: {ex.Message}");
        }
    }
}
