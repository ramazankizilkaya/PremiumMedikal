using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class SubCategory:BaseModel
    {
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
        }

        [Required, MaxLength(100)]
        public string NameTR { get; set; }

        [Required, MaxLength(100)]
        public string NameEN { get; set; }

        [Required, MaxLength(100)]
        public string NameRU { get; set; }

        public string SubCategoryNameUrlTR { get; set; }
        public string SubCategoryNameUrlEN { get; set; }
        public string SubCategoryNameUrlRU { get; set; }

        public string HeadTitleTR { get; set; }
        public string HeadTitleEN { get; set; }
        public string HeadTitleRU { get; set; }
        public string HeadDescriptionTR { get; set; }
        public string HeadDescriptionEN { get; set; }
        public string HeadDescriptionRU { get; set; }
        public CategoryType CategoryType { get; set; }


        [ForeignKey("MiddleCategory")]
        public int MiddleCategoryId { get; set; }
        public virtual MiddleCategory MiddleCategory { get; set; }

        //[ForeignKey("TopCategory")]
        //public int? TopCategoryId { get; set; }
        //public virtual TopCategory TopCategory { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
