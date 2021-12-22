using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        [Required(ErrorMessage ="Isim Alanı Gereklidir.")]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage ="Soyad Alanı Gereklidir.")]
        [Display(Name="Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required(ErrorMessage ="E-posta Alanı Gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Şifre Alanı Gereklidir.")]
        [StringLength(100,MinimumLength = 6, ErrorMessage ="Şifreniz En Az 6 Karakterli olmalıdır.")]
        [Display(Name="Şifre")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
