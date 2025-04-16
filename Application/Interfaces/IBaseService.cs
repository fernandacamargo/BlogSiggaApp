namespace BlogSiggaApp.Application.Interfaces
{
    public interface IBaseService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);


    }
}