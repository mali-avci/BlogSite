using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parolanız eşleşmiyor.")]
        [Display(Name = "Parola")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Hakkımda")]
        public string About { get; set; }

        public bool IsUpdate { get; set; }
    }
}
