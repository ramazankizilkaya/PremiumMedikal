using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminAdProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen Reklam dilini seçiniz.")]
        public string Culture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string AdproHref { get; set; }

        [Required(ErrorMessage = "Lütfen Reklam hedef türünü seçiniz.")]
        public SliderTargetType AdproTargetType { get; set; }

        public int? TargetTopCategoryId { get; set; }
        public int? TargetMiddleCategoryId { get; set; }
        public int? TargetSubCategoryId { get; set; }
        public int? TargetProductId { get; set; }
        public int? TargetBrandId { get; set; }

        public string TargetProductName { get; set; }
        public string GenericTargetName { get; set; }
        public IFormFile AdProductPhoto { get; set; }
       
        public List<TopCategory> TopCategories { get; set; }
        public List<MiddleCategory> MiddleCategories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Brand> Brands { get; set; }

    }
}
