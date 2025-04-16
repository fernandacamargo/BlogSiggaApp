namespace BlogSiggaApp.Application.Interfaces
{
    public interface IFileStreamService
    {
        Task<Stream> GetFileStreamAsync();
    }
}
