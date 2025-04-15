using BlogProject.Entity;

public interface IPostRepository
{
    IQueryable<Post> Post { get; }

    void CreatePost(Post post);
    void EditPost(Post post);

    Task SaveAsync();
    Task DeletePost(Post post); 
}
