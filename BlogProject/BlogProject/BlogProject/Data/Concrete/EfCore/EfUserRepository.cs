using BlogProject.Data.Abstract;
using BlogProject.Entity;

namespace BlogProject.Data.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(Entity.User user)
        {
          
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
