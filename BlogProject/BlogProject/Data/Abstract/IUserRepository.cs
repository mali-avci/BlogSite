using BlogProject.Entity;

namespace BlogProject.Data.Abstract
{
    public interface IUserRepository 
    {

        IQueryable<User> Users { get; }
        void CreateUser(User user);
        void UpdateUser(Entity.User user);
    }
}
