using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Application.Services;
using BlogSiggaApp.Domain.Interfaces;
using BlogSiggaApp.Infra;
using BlogSiggaApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlogSiggaApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug() 
               .WriteTo.Console()    
               .WriteTo.File("logs\\app-log.txt", rollingInterval: RollingInterval.Day) 
               .CreateLogger();


            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG


            // Adicionando logger para debug
            builder.Logging.AddDebug();

            //Serilog
            builder.Logging.AddSerilog();

            //Repository web
            builder.Services.AddHttpClient<IWebServiceRepository, WebServiceRepository>((serviceProvider, client) =>
            {
                // Obter a configuração usando o IConfiguration
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var baseUrl = AppSettings.ApiBaseUrl; 

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);

            });


            //Repository db
            builder.Services.AddSingleton<IPostRepository>(sp =>
            {

                var dbPath = AppSettings.DatabasePath;
                return new PostRepository(dbPath);
            });

            // Registrar os outros serviços          
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddTransient<IBaseService, BaseService>();
            builder.Services.AddSingleton<IPostService, PostService>();
            builder.Services.AddTransient<IApiService, ApiService>();

            // Registrar o ViewModel
            builder.Services.AddTransient<PostsViewModel>();

            // Registrar Page
            builder.Services.AddTransient<MainPage>();
                  

#endif

            return builder.Build();
        }
    }
}
