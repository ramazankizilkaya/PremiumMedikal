using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Enums
{
    public enum ProductOfferType
    {
        [Display(Name="Normal Ürün")]
        Nothing=0,
        [Display(Name = "Vitrin Ürünleri")]
        ShowcaseProduct,
        [Display(Name = "Kampanyalı Ürünler")]
        CampaignProduct,
        [Display(Name = "İndirimdekiler")]
        OnSaleProduct,
        [Display(Name = "Haftanın Ürünleri")]
        WeeklyDealProduct,
        [Display(Name = "Yeni Ürünler")]
        NewProduct
    }
}
