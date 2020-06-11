using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminCreateSliderDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen Slider dilini seçiniz.")]
        public string Culture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        //[Required(ErrorMessage = "Lütfen Slider fotoğrafı alt tag ini giriniz.")]
        public string PhotoAltTag { get; set; }

        //[Required(ErrorMessage = "Lütfen Sliderın hedef ürün adını giriniz.")]
        public string TargetProductName { get; set; }
        public string SliderHref { get; set; }

        [Required(ErrorMessage = "Lütfen slider hedef türünü giriniz.")]
        public SliderTargetType SliderTargetType { get; set; }

        public int? TargetTopCategoryId { get; set; }
        public int? TargetMiddleCategoryId { get; set; }
        public int? TargetSubCategoryId { get; set; }
        public int? TargetProductId { get; set; }
        public int? TargetBrandId { get; set; }

        [Required(ErrorMessage = "Lütfen slider ana fotoğrafını giriniz.")]
        public IFormFile SliderPhoto { get; set; }

        [Required(ErrorMessage = "Lütfen slider thumb fotoğrafını giriniz.")]
        public IFormFile SliderThumbPhoto { get; set; }
       
        public List<TopCategory> TopCategories { get; set; }
        public List<MiddleCategory> MiddleCategories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Brand> Brands { get; set; }
    }
}
