
using BlogProject.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace BlogProject.ViewComponents
{
    public class TagsMenu : ViewComponent
    {

        private ITagRepository _tagRepository;

        public TagsMenu(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagRepository.Tags.ToListAsync());
        }
    }
}