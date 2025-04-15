using System.Security.Claims;
using BlogProject.Models;
using BlogProject.Data.Abstract;
using BlogProject.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserRepository _userRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, IWebHostEnvironment hostEnvironment, ITagRepository tagRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _hostEnvironment = hostEnvironment;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        // Index: Posts tablosunu filtreleyerek g�r�nt�ler
        public async Task<IActionResult> Index(string tag, int page = 1)
        {
            const int pageSize = 5; 

            
            var postsQuery = _postRepository.Post.Include(p => p.Tag).Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                // Many-to-many yerine tek etiket �zerinden filtreleme
                postsQuery = postsQuery.Where(x => x.Tag.Url == tag);
            }

            var totalPosts = await postsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

            var posts = await postsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PostViewModel
            {
                Posts = posts,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        // Details: Post detay�n� g�sterir
        public async Task<IActionResult> Details(string url)
        {
           
            var post = await _postRepository.Post
                .Include(x => x.Tag)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(p => p.Url == url);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // Yorum ekler
        [HttpPost]
        public IActionResult AddComment(int PostId, string Text)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = _userRepository.Users.FirstOrDefault(u => u.UserId == userId);

            var comment = new Comment
            {
                PostId = PostId,
                UserId = userId,
                Text = Text,
                PublishedOn = DateTime.Now
            };

            _commentRepository.CreateComment(comment);

            
            var avatar = string.IsNullOrEmpty(user.Image) ? "default-profile.jpg" : user.Image;

            return Json(new
            {
                username = user.UserName,
                avatar = avatar,
                text = comment.Text,
                publishedOn = comment.PublishedOn
            });
        }


        // Create: Yeni post olu�turur
        [Authorize]
        public IActionResult Create()
        {
            var tags = _tagRepository.Tags.ToList();
            var model = new PostCreateViewModel
            {
                
                AvailableTags = tags.Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(), 
                    Text = t.Text ?? "Bilinmiyor"
                }).ToList(),
                IsActive = true
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTags = _tagRepository.Tags.Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.Text ?? "Bilinmiyor"
                }).ToList();

                return View(model);  // Hatal� model ile ayn� sayfaya geri d�n
            }

            // G�rsel y�kleme i�lemi
            string uniqueFileName = "default.jpg";
            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);  
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
            }

            // Kullan�c� ID'sini al
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            
            int selectedTagId = model.SelectedTags.FirstOrDefault();

            var post = new Post
            {
                Title = model.Title,
                Description = model.Description,
                Url = model.Url,
                Content = model.Content,
                PublishedOn = DateTime.Now,
                UserId = userId,
                Image = uniqueFileName,
                IsActive = model.IsActive,
                TagId = selectedTagId  
            };

            _postRepository.CreatePost(post);

            return RedirectToAction("List");
        }

        // List: Kullan�c�n�n kendi postlar�n� listeler
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Posts verilerini al�yoruz
            var posts = _postRepository.Post.AsQueryable();

            // Kullan�c� sadece kendi postlar�n� g�rmeli, admin ise t�m postlar� g�rmeli
            if (role != "admin")
            {
                posts = posts.Where(i => i.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Post.FirstOrDefault(i => i.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            // Admin de�ilse ve post kendisine ait de�ilse -> eri�im yasak
            if (role != "admin" && post.UserId != userId)
            {
                return Forbid();
            }

            // Etiketleri al
            var tags = _tagRepository.Tags.ToList();

            var selectedTagIds = new List<int> { post.TagId };

            var model = new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                UserId = post.UserId,
                AvailableTags = tags.Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.Text ?? "Bilinmiyor"
                }).ToList(),
                SelectedTags = selectedTagIds,
                ImageFile = null 
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
                var role = User.FindFirstValue(ClaimTypes.Role);

                var existingPost = _postRepository.Post.FirstOrDefault(p => p.PostId == model.PostId);
                if (existingPost == null)
                {
                    return NotFound();
                }

                // Yetki kontrol�: Admin de�ilse ve post sahibi de�ilse d�zenleme izni yok.
                if (role != "admin" && existingPost.UserId != currentUserId)
                {
                    return Forbid();
                }

                // Ortak alanlar: ba�l�k, a��klama, i�erik, url g�ncelle.
                existingPost.Title = model.Title;
                existingPost.Description = model.Description;
                existingPost.Content = model.Content;
                existingPost.Url = model.Url;

                if (role == "admin" || existingPost.UserId == currentUserId)
                {
                    existingPost.IsActive = model.IsActive;
                }

                // Foto�raf g�ncelleme: 
                // E�er yeni bir foto�raf y�klenmi�se, g�ncelle; yoksa eski foto�raf� koru.
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, newFileName);

                    // Eski foto�raf varsa sil
                    var oldFilePath = Path.Combine(uploadsFolder, existingPost.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    existingPost.Image = newFileName;
                }
               

                // Tek etiket kullan�m�: Se�ilen etiketlerden (SelectedTags listesi) ilk e�le�en Tag ata
                existingPost.Tag = _tagRepository.Tags.FirstOrDefault(t => model.SelectedTags.Contains(t.TagId));

                _postRepository.EditPost(existingPost);
                await _postRepository.SaveAsync();

                return RedirectToAction("List");
            }

            // ModelState hatal�ysa AvailableTags listesini g�ncelle
            model.AvailableTags = _tagRepository.Tags
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.Text ?? "Bilinmiyor" })
                .ToList();

            return View(model);
        }

        // SetPostStatus: Kullan�c� yaln�zca kendi postunun durumunu de�i�tir
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetPostStatus(int postId, bool isActive)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var post = await _postRepository.Post.FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId != userId && User.FindFirstValue(ClaimTypes.Role) != "admin")
            {
                return Unauthorized();
            }

            post.IsActive = isActive;
            _postRepository.EditPost(post);
            await _postRepository.SaveAsync();

            return RedirectToAction("List");
        }

        // Delete: Postu siler
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int postId)
        {
            var post = await _postRepository.Post.FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            try
            {
                await _postRepository.DeletePost(post);
                await _postRepository.SaveAsync();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = "Post silinirken bir hata olu�tu.",
                    ExceptionMessage = ex.Message
                };

                return View("Error", errorModel);
            }
        }
        [HttpGet("api/posts/suggestions")]
        public async Task<JsonResult> Suggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new List<string>());

            var suggestions = await _postRepository.Post
                .Where(p => p.IsActive && p.Title.Contains(q))
                .OrderBy(p => p.Title)
                .Select(p => p.Title)
                .Take(10)
                .ToListAsync();

            return Json(suggestions);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string q, int page = 1)
        {
            ViewData["SearchQuery"] = q;

            const int pageSize = 5;
            var query = _postRepository.Post
                .Where(p => p.IsActive &&
                           (p.Title.Contains(q) ||
                            p.Description.Contains(q) ||
                            p.Content.Contains(q)))
                .OrderByDescending(p => p.PublishedOn);

            var total = await query.CountAsync();
            var posts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PostViewModel
            {
                Posts = posts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
            return View("SearchResults", model);
        }



    }
}
