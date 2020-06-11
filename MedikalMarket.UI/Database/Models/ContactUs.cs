using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Database.Models
{
    public class ContactUs:BaseModel
    {
        [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
        [MinLength(2, ErrorMessage = "Ad ve soyad en az 2 karakter içermelidir.")]
        [MaxLength(100, ErrorMessage = "Ad ve soyad en fazla 100 karakter içermelidir.")]
        [Display(Name = "Adınız ve Soyadınız")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [MaxLength(100, ErrorMessage = "Eposta adresi en fazla 100 karakter içermelidir.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta Adresiniz")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Lütfen konu giriniz")]
        [MinLength(2, ErrorMessage = "Konu en az 2 karakter içermelidir.")]
        [MaxLength(100, ErrorMessage = "Konu en fazla 100 karakter içermelidir.")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Lütfen mesajınızı yazınız")]
        [MinLength(20, ErrorMessage = "Mesajınız en az 2 karakter içermelidir.")]
        [MaxLength(4000, ErrorMessage = "Mesajınız en fazla 4000 karakter içermelidir.")]
        [Display(Name = "Mesajınız")]
        public string Message { get; set; }
    }
}
