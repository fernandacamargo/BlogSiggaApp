using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Application.Interfaces
{
    public interface IWebService
    {
        Task<List<Post>> GetDataAsync();
    }
}
