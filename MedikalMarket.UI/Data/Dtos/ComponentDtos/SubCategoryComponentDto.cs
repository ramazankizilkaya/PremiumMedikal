using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class SubCategoryComponentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubCategoryNameUrl { get; set; }
        public string HeadTitle { get; set; }
        public string HeadDescription { get; set; }
        public string CategoryHref { get; set; }

        public int MiddleCategoryId { get; set; }
        public virtual MiddleCategoryComponentDto MiddleCategoryComponentDto { get; set; }
    }
}
