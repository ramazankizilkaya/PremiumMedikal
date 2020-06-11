using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class Admin:BaseModel
    {
        [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
        [MinLength(2, ErrorMessage = "Ad ve soyad minimum 2 karakter içermelidir.")]
        [MaxLength(100, ErrorMessage = "Ad ve soyad maksimum 100 karakter içermelidir.")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [MaxLength(100, ErrorMessage = "Eposta adresi maksimum 100 karakter içermelidir.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        [Remote(action: "ValidateEmail", controller: "Auth", ErrorMessage = "Girmiş olduğunuz e-posta adresi sistemimizde kayıtlıdır.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [MinLength(5, ErrorMessage = "Şifreniz minimum 5 karakter içermelidir.")]
        [MaxLength(20, ErrorMessage = "Şifreniz maksimum 20 karakter içermelidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "Hatalı giriş yaptınız. Lütfen tekrar deneyiniz. Örnek(532 176 83 35)")]
        public string CellPhoneNumber { get; set; }
    }
}
