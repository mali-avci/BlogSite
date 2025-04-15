using BlogProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Data.Concrete.EfCore
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Tag)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
