using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Enums
{
    public enum SliderTargetType
    {
        [Display(Name = "Alt Kategori Sayfası")]
        SubCategory=0,
        [Display(Name = "Ana Kategori Sayfası")]
        TopCategory,
        [Display(Name = "Marka Ana Sayfası")]
        BrandMasterPage,
        [Display(Name = "Orta Kategori Sayfası")]
        MiddleCategory,
        [Display(Name = "Ürün Detay Sayfası")]
        SingleProduct
    }
}
