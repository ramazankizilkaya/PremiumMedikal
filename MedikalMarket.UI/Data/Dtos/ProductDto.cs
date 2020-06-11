using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductNameUrl { get; set; }
        public string MainPhotoFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public ProductOfferType ProductOfferType { get; set; }
        public bool HasNewBadge { get; set; }
        public bool IsFreeShipping { get; set; }
        public string BrandName { get; set; }
        public string BrandNameUrl { get; set; }
        public string ProductHref { get; set; }
    }
}
