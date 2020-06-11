using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class MiddleCategoryComponentDto
    {
        public MiddleCategoryComponentDto()
        {
            this.SubCategoryComponentDtos = new HashSet<SubCategoryComponentDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MiddleCategoryNameUrl { get; set; }
        public string HeadTitle { get; set; }
        public string HeadDescription { get; set; }
        public string CategoryHref { get; set; }

        public int TopCategoryId { get; set; }
        public virtual TopCategoryComponentDto TopCategoryComponentDto { get; set; }

        public virtual ICollection<SubCategoryComponentDto> SubCategoryComponentDtos { get; set; }
    }
}
