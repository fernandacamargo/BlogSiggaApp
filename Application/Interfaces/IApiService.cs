using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Application.Interfaces
{
    public interface IApiService
    {
        Task<List<Post>> FetchPostsAsync();
    }
}
