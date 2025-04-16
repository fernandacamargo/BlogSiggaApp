using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int postId);
        Task AddPostAsync(Post post);
        List<Post> GetLocalPostsAsync();
        void SavePostsAsync(List<Post> posts);
        void InsertPostsAsync(List<Post> newPosts); // <- novo método
    }
}
