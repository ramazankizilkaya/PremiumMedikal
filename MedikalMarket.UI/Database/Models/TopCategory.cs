using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class TopCategory:BaseModel
    {
        public TopCategory()
        {
            this.MiddleCategories = new HashSet<MiddleCategory>();
            this.Products = new HashSet<Product>();
        }

        [Required, MaxLength(200)]
        public string NameTR { get; set; }

        [Required, MaxLength(200)]
        public string NameEN { get; set; }

        [Required, MaxLength(200)]
        public string NameRU { get; set; }
        public bool HasMiddleCategories { get; set; }
        public string TopCategoryNameUrlTR { get; set; }
        public string TopCategoryNameUrlRU { get; set; }
        public string TopCategoryNameUrlEN { get; set; }

        public string HeadTitleTR { get; set; }
        public string HeadTitleEN { get; set; }
        public string HeadTitleRU { get; set; }
        public string HeadDescriptionTR { get; set; }
        public string HeadDescriptionEN { get; set; }
        public string HeadDescriptionRU { get; set; }

        public CategoryType CategoryType { get; set; }
        public virtual ICollection<MiddleCategory> MiddleCategories { get; set; }
        //public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
