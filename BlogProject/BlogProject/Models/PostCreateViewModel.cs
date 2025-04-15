using BlogProject.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Models
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        [Required]
        [Display(Name = "Post Görünürlük Durumu")]
        public bool IsActive { get; set; }

        public int UserId { get; set; }

        
        [Display(Name = "Post için görsel")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required]
        [Display(Name = "Post için ilgili kategorileri seç")]
        public List<int> SelectedTags { get; set; } = new();  

        [Required]
        [Display(Name = "Tüm kategoriler:")]
        public List<SelectListItem> AvailableTags { get; set; } = new();  
    }

}
