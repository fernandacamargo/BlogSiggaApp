using BlogSiggaApp.Application.Interfaces;
using Serilog;

namespace BlogSiggaApp.Application.Services
{
    public class BaseService : IBaseService
    {

        public void LogInformation(string message)
        {

            Log.Information(message);
        }


        public void LogWarning(string message)
        {

            Log.Warning(message);
        }

        public void LogError(string message)
        {
                  
            Log.Error(message);
        }

    }
}
