using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class FavoriteProductDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductNameUrl { get; set; }
        public string MainPhotoFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string BrandName { get; set; }
        public string BrandNameUrl { get; set; }
        public string TopCategoryName { get; set; }
        public string TopCategoryNameUrl { get; set; }
        public string MiddleCategoryName { get; set; }
        public string MiddleCategoryNameUrl { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryNameUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BrandHref { get; set; }
        public string ProductHref { get; set; }

    }
}
