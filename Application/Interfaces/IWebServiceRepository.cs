using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Application.Interfaces
{
    public interface IWebServiceRepository
    {
        Task<List<Post>> GetDataAsync();
    }
}
