using BlogProject.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private readonly IPostRepository _postRepository;

        public NewPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var posts = await _postRepository.Post
                .Where(p => p.IsActive) 
                .OrderByDescending(p => p.PublishedOn)
                .Take(5)
                .ToListAsync();

            return View(posts);
        }
    }
}
