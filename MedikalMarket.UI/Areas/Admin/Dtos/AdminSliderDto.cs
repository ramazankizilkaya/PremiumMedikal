using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminSliderDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen slider dilini seçiniz.")]
        public string Culture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string PhotoFileName { get; set; }
        public string ThumbFileName { get; set; }

        //[Required(ErrorMessage = "Lütfen slider fotoğrafı alt tag ini giriniz.")]
        public string PhotoAltTag { get; set; }
        public string SliderHref { get; set; }

        [Required(ErrorMessage = "Lütfen slider hedef türünü seçiniz.")]
        public SliderTargetType SliderTargetType { get; set; }

        public int? TargetTopCategoryId { get; set; }
        public int? TargetMiddleCategoryId { get; set; }
        public int? TargetSubCategoryId { get; set; }
        public int? TargetProductId { get; set; }
        public int? TargetBrandId { get; set; }

        //[Required(ErrorMessage = "Lütfen Sliderın hedef ürün adını giriniz.")]
        public string TargetProductName { get; set; }

        public string GenericTargetName { get; set; }
        public IFormFile SliderPhoto { get; set; }
        public IFormFile SliderThumbPhoto { get; set; }
       
        public List<TopCategory> TopCategories { get; set; }
        public List<MiddleCategory> MiddleCategories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Brand> Brands { get; set; }

        //public int? SelectedTopCategoryId { get; set; }
        //public int? SelectedMiddleCategoryId { get; set; }
        //public int? SelectedSubCategoryId { get; set; }
        //public int? SelectedProductId { get; set; }
        //public int? SelectedBrandId { get; set; }

    }
}
