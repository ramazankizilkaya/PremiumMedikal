using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class ProductTabsDto
    {
        public ProductTabsDto()
        {
            CampaignProducts = new HashSet<ShowcaseProductsDto>();
            OnSaleProducts = new HashSet<ShowcaseProductsDto>();
            WeeklyDealProducts = new HashSet<ShowcaseProductsDto>();
            NewProducts = new HashSet<ShowcaseProductsDto>();
        }
        public ICollection<ShowcaseProductsDto> CampaignProducts { get; set; }
        public ICollection<ShowcaseProductsDto> OnSaleProducts { get; set; }
        public ICollection<ShowcaseProductsDto> WeeklyDealProducts { get; set; }
        public ICollection<ShowcaseProductsDto> NewProducts { get; set; }

        public string CampaignProductsTitle { get; set; }
        public string OnSaleProductsTitle { get; set; }
        public string WeeklyDealProductsTitle { get; set; }
        public string NewProductsTitle { get; set; }
        public string NewBadgeText { get; set; }
        public string FreeShippingText { get; set; }
    }
}
