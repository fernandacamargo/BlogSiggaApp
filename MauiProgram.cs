using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Application.Services;
using BlogSiggaApp.Domain.Interfaces;
using BlogSiggaApp.Infra.Http;
using BlogSiggaApp.Infra.Persistence;
using BlogSiggaApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlogSiggaApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG

            builder.Services.AddHttpClient<IWebService, WebService>((serviceProvider, client) =>
            {
                // Obter a configuração usando o IConfiguration
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var baseUrl = AppSettings.ApiBaseUrl; 

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            });

            builder.Services.AddSingleton<IPostRepository>(sp =>
            {

                var dbPath = AppSettings.DatabasePath;
                return new PostRepository(dbPath);
            });

            // Registrar os outros serviços          
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddSingleton<IPostService, PostService>();
            builder.Services.AddTransient<IApiService, ApiService>();


            // Registrar o ViewModel
            builder.Services.AddTransient<PostsViewModel>();

            // Registrar Page
            builder.Services.AddTransient<MainPage>();

            // Adicionando logger para debug
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
