using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class MiddleCategory:BaseModel
    {
        public MiddleCategory()
        {
            this.SubCategories = new HashSet<SubCategory>();
            this.Products = new HashSet<Product>();
        }

        [Required, MaxLength(100)]
        public string NameTR { get; set; }

        [Required, MaxLength(100)]
        public string NameEN { get; set; }

        [Required, MaxLength(100)]
        public string NameRU { get; set; }
        public string MiddleCategoryNameUrlTR { get; set; }
        public string MiddleCategoryNameUrlEN { get; set; }
        public string MiddleCategoryNameUrlRU { get; set; }

        public bool HasSubCategories { get; set; }

        [ForeignKey("TopCategory")]
        public int TopCategoryId { get; set; }
        public virtual TopCategory TopCategory { get; set; }

        public string HeadTitleTR { get; set; }
        public string HeadTitleEN { get; set; }
        public string HeadTitleRU { get; set; }
        public string HeadDescriptionTR { get; set; }
        public string HeadDescriptionEN { get; set; }
        public string HeadDescriptionRU { get; set; }
        public CategoryType CategoryType { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
