using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using System.Collections.Generic;

namespace MedikalMarket.UI.Data.Dtos
{
    public class SingleProductDto
    {
        public SingleProductDto()
        {
            ProductPhotoDtos = new HashSet<SingleProductPhotoDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public string ProductNameUrl { get; set; }
        public string MainPhotoFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string ProductCode { get; set; }
        public int? DiscountRate { get; set; }
        public bool HasNewBadge { get; set; }
        public bool IsFreeShipping { get; set; }
        public string HeadTitle { get; set; }
        public string HeadDescription { get; set; }
        public string BrandName { get; set; }
        public string TopCategoryName { get; set; }
        public string TopCategoryNameUrl { get; set; }
        public string MiddleCategoryName { get; set; }
        public string MiddleCategoryNameUrl { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryNameUrl { get; set; }
        public bool IsFavorite { get; set; }
        public ProductOfferType ProductOfferType { get; set; }

        public virtual ICollection<SingleProductPhotoDto> ProductPhotoDtos { get; set; }
    }
}
