using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;
using BlogSiggaApp.Domain.Interfaces;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;


    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public Task<List<Post>> GetPostsAsync()
    {
        return _postRepository.GetPostsAsync();
    }

    public Task<Post> GetPostByIdAsync(int postId)
    {
        return _postRepository.GetPostByIdAsync(postId);
    }

    public Task AddPostAsync(Post post)
    {
        return _postRepository.AddPostAsync(post);
    }

    public List<Post> GetLocalPostsAsync()
    {
        return _postRepository.GetLocalPostsAsync();
    }

    public void SavePostsAsync(List<Post> posts)
    {
        _postRepository.SavePostsAsync(posts);
    }

    public void InsertPostsAsync(List<Post> newPosts)
    {
        _postRepository.InsertPostsAsync(newPosts);
    }
}
