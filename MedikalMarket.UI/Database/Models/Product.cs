    using MedikalMarket.UI.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedikalMarket.UI.Database.Models
{
    public class Product:BaseModel
    {
        public Product()
        {
            ProductPhotos = new HashSet<ProductPhoto>();
            FavoriteProducts = new HashSet<FavoriteProduct>();
        }

        [Required]
        public string NameTR { get; set; }
        [Required]
        public string NameEN { get; set; }
        [Required]
        public string NameRU { get; set; }
        [Required]
        public string ProductDescriptionTR { get; set; }
        [Required]
        public string ProductDescriptionEN { get; set; }
        [Required]
        public string ProductDescriptionRU { get; set; }
        public string ProductNameUrlTR { get; set; }
        public string ProductNameUrlEN { get; set; }
        public string ProductNameUrlRU { get; set; }
        public string MainPhotoFileName { get; set; }
        public string ProductCode { get; set; }
        public int? DiscountRate { get; set; }
        public int? CountOfVisit { get; set; }
        public int? CountOfFavorite { get; set; }
        public int? CountOfPriceAlarm { get; set; }
        public int? StockNumber { get; set; }
        public int? NumberInStock { get; set; }

        public ProductOfferType ProductOfferType { get; set; }
        public bool HasNewBadge { get; set; }
        public bool IsFreeShipping { get; set; }

        [Required]
        public string HeadTitleTR { get; set; }
        [Required]
        public string HeadTitleEN { get; set; }
        [Required]
        public string HeadTitleRU { get; set; }
        [Required]
        public string HeadDescriptionTR { get; set; }
        [Required]
        public string HeadDescriptionEN { get; set; }
        [Required]
        public string HeadDescriptionRU { get; set; }

        [Required]
        public string PhotoAltTagTR { get; set; }
        [Required]
        public string PhotoAltTagEN { get; set; }
        [Required]
        public string PhotoAltTagRU { get; set; }


        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        [ForeignKey("TopCategory")]
        public int TopCategoryId { get; set; }
        public virtual TopCategory TopCategory { get; set; }

        [ForeignKey("MiddleCategory")]
        public int? MiddleCategoryId { get; set; }
        public virtual MiddleCategory MiddleCategory { get; set; }

        [ForeignKey("SubCategory")]
        public int? SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }

    }
}
