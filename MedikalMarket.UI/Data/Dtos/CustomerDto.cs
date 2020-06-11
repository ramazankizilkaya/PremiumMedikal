using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            FavoriteProductDtos = new HashSet<FavoriteProductDto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı giriniz.")]
        [MinLength(2, ErrorMessage = "Ad ve soyad minimum 2 karakter içermelidir.")]
        [MaxLength(100, ErrorMessage = "Ad ve soyad maksimum 100 karakter içermelidir.")]
        public string NameSurname { get; set; }

        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [MaxLength(100, ErrorMessage = "Eposta adresi maksimum 100 karakter içermelidir.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [MinLength(5, ErrorMessage = "Şifreniz minimum 5 karakter içermelidir.")]
        [MaxLength(20, ErrorMessage = "Şifreniz maksimum 20 karakter içermelidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz.")]
        [Compare(nameof(Password), ErrorMessage = "Girdiğiniz şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string CellPhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsSubscribedToSMS { get; set; }
        public bool IsSubscribedToEmail { get; set; }

        public ICollection<FavoriteProductDto> FavoriteProductDtos { get; set; }
    }
}
