using BlogProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Post => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;
                _context.SaveChanges();
            }
        }

   
        
        public async Task DeletePost(Post post)
        {
           
            var entity = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == post.PostId);

            if (entity != null)
            {
                
                var comments = await _context.Comments.Where(c => c.PostId == post.PostId).ToListAsync();

                _context.Comments.RemoveRange(comments);

                _context.Posts.Remove(entity);

                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Post bulunamadı.");
            }
        }



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
