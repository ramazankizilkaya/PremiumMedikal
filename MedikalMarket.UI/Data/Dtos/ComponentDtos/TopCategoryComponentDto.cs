using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class TopCategoryComponentDto
    {
        public TopCategoryComponentDto()
        {
            MiddleCategoryComponentDtos = new HashSet<MiddleCategoryComponentDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string TopCategoryNameUrl { get; set; }

        public string HeadTitle { get; set; }
        public string HeadDescription { get; set; }

        public virtual ICollection<MiddleCategoryComponentDto> MiddleCategoryComponentDtos { get; set; }
    }
}
