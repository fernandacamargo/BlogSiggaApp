using BlogSiggaApp.Domain.Entities;
using BlogSiggaApp.Domain.Interfaces;
using SQLite;

namespace BlogSiggaApp.Infra.Persistence
{
    public class PostRepository : IPostRepository
    {
        private readonly SQLiteConnection _db;

        public PostRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Post>();
        }

        public Task<List<Post>> GetPostsAsync()
        {
            return Task.FromResult(_db.Table<Post>().ToList());
        }

        public Task SavePostsAsync(List<Post> posts)
        {
            _db.DeleteAll<Post>();
            _db.InsertAll(posts);
            return Task.CompletedTask;
        }
      

        public Task<Post> GetPostByIdAsync(int postId)
        {
            return Task.FromResult(_db.Table<Post>().FirstOrDefault(p => p.Id == postId));
        }

        public Task AddPostAsync(Post post)
        {
            _db.Insert(post);
            return Task.CompletedTask;
        }

        public List<Post> GetLocalPostsAsync()
        {
            return _db.Table<Post>().ToList();
        }

        public void InsertPostsAsync(List<Post> newPosts)
        {
            foreach (var post in newPosts)
            {
                // Evita inserções duplicadas por precaução
                var exists = _db.Table<Post>().Where(p => p.Id == post.Id).FirstOrDefault();
                if (exists == null)
                {
                    _db.Insert(post);
                }
            }
        }
    }
}
