using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class CategorySidebarDto
    {
        public CategorySidebarDto()
        {
            MiddleCategoryComponentDtos = new HashSet<MiddleCategoryComponentDto>();
            SubCategoryComponentDtos = new HashSet<SubCategoryComponentDto>();
        }

        public ICollection<MiddleCategoryComponentDto> MiddleCategoryComponentDtos { get; set; }
        public ICollection<SubCategoryComponentDto> SubCategoryComponentDtos { get; set; }
        public int TopCategoryId { get; set; }
        public string CategoryType { get; set; }
        public string ComponentTitle { get; set; }
    }
}
