using System.Security.Claims;
using BlogProject.Data.Abstract;
using BlogProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;

        public UsersController(IUserRepository userRepository, IWebHostEnvironment environment)
        {
            _userRepository = userRepository;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Post");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(string? username)
        {
            var model = new RegisterViewModel();

            if (!string.IsNullOrEmpty(username))
            {
                var user = _userRepository.Users.FirstOrDefault(x => x.UserName == username);
                if (user != null)
                {
                    model.Name = user.Name;
                    model.Username = user.UserName;
                    model.Email = user.Email;
                    model.IsUpdate = true;
                    model.About = user.About;
                    ViewData["Title"] = "Profil Bilgilerini Güncelle";
                    return View(model);
                }
            }

            ViewData["Title"] = "Kayıt Ol";
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (model.IsUpdate)
                {
                    // Güncelleme modunda: Mevcut kullanıcıyı bul ve bilgileri güncelle
                    var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.Username);
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        user.Password = model.Password;
                        user.About = model.About;
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            var extension = Path.GetExtension(ImageFile.FileName);
                            string imageName = Guid.NewGuid().ToString() + extension;
                            var path = Path.Combine(_environment.WebRootPath, "img", imageName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await ImageFile.CopyToAsync(stream);
                            }
                            user.Image = imageName;
                        }
                        _userRepository.UpdateUser(user);
                        
                        return RedirectToAction("Profile", new { username = model.Username });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Güncellenecek kullanıcı bulunamadı.");
                    }
                }
                else
                {
                    // Yeni kayıt modunda: Kullanıcı adı veya email kullanımda mı diye kontrol et
                    var existingUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.Username || x.Email == model.Email);
                    if (existingUser == null)
                    {
                        string imageName = "default.jpg";
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            var extension = Path.GetExtension(ImageFile.FileName);
                            imageName = Guid.NewGuid().ToString() + extension;
                            var path = Path.Combine(_environment.WebRootPath, "img", imageName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await ImageFile.CopyToAsync(stream);
                            }
                        }

                        _userRepository.CreateUser(new Entity.User
                        {
                            UserName = model.Username,
                            Name = model.Name,
                            Email = model.Email,
                            Password = model.Password,
                            About = model.About,
                            Image = imageName
                        });
                        TempData["SuccessMessage"] = "Başarıyla kayıt oldunuz. Giriş yapabilirsiniz.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username ya da Email kullanımda.");
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

                if (isUser != null)
                {
                    // Kullanıcının profil fotoğrafı için tam URL'yi oluştur
                    var profileImageUrl = string.IsNullOrEmpty(isUser.Image)
                        ? Url.Content("~/img/default-profile.jpg")
                        : Url.Content("~/img/" + isUser.Image);

                    // Claim'lere kullanıcı bilgilerini ekliyoruz.
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, isUser.UserName ?? ""),
                        new Claim(ClaimTypes.GivenName, isUser.Name ?? ""),
                        // Özel claim ile fotoğraf yolunu ekle
                        new Claim("ProfileImageUrl", profileImageUrl)
                    };

                    if (isUser.Email == "admin@blog.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Profile(string username)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userRepository.Users
                .Include(x => x.Posts)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Post)
                .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userRepository.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
