using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminBrandDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage ="Lütfen marka adını giriniz.")]
        public string BrandName { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoAltTagTR { get; set; }
        public string PhotoAltTagEN { get; set; }
        public string PhotoAltTagRU { get; set; }
        public string BandNameUrl { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının Türkçe title etiket açıklamasını giriniz.")]
        public string MasterPageMetaTitleTR { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının İngilizce title etiket açıklamasını giriniz.")]
        public string MasterPageMetaTitleEN { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının Rusça title etiket açıklamasını giriniz.")]
        public string MasterPageMetaTitleRU { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının Türkçe description etiket açıklamasını giriniz.")]
        public string MasterPageMetaDescriptionTR { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının İngilizce description etiket açıklamasını giriniz.")]
        public string MasterPageMetaDescriptionEN { get; set; }

        [Required(ErrorMessage = "Lütfen marka ürün sayfasının Rusça description etiket açıklamasını giriniz.")]
        public string MasterPageMetaDescriptionRU { get; set; }

        public int ProductCount { get; set; }
        public IFormFile BrandPhoto { get; set; }
    }
}
