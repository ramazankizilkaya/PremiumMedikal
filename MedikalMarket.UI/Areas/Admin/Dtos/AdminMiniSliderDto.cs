using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminMiniSliderDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Lütfen Slider kültürünü seçiniz.")]
        public string Culture { get; set; }
        public string PhotoFileName { get; set; }

        public string PhotoAltTag { get; set; }
        public string SliderHref { get; set; }

        //[Required(ErrorMessage = "Lütfen Slider fotoğrafı seçiniz.")]
        public IFormFile SliderPhoto { get; set; }
        public int TargetProductId { get; set; }

        [Required(ErrorMessage = "Lütfen Sliderın hedef ürün adını giriniz.")]
        public string TargetProductName { get; set; }
    }
}
