using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task SavePostsAsync(List<Post> posts);
        Task<Post> GetPostByIdAsync(int postId);
        Task AddPostAsync(Post post);
        List<Post> GetLocalPostsAsync();
        void InsertPostsAsync(List<Post> newPosts);

    }
}
