using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminCreateBrandDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage ="Lütfen marka adını giriniz.")]
        [Remote("ValidateBrandName", "Brand", HttpMethod = "Post", ErrorMessage = "Girmiş olduğunuz marka adı sistemimizde kayıtlıdır.")]
        public string BrandName { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoAltTagTR { get; set; }
        public string PhotoAltTagEN { get; set; }
        public string PhotoAltTagRU { get; set; }
        public string BandNameUrl { get; set; }
        public string MasterPageMetaTitleTR { get; set; }
        public string MasterPageMetaTitleEN { get; set; }
        public string MasterPageMetaTitleRU { get; set; }
        public string MasterPageMetaDescriptionTR { get; set; }
        public string MasterPageMetaDescriptionEN { get; set; }
        public string MasterPageMetaDescriptionRU { get; set; }
        public int ProductCount { get; set; }

        [Required(ErrorMessage = "Lütfen marka fotoğrafını seçiniz.")]
        public IFormFile BrandPhoto { get; set; }
    }
}
