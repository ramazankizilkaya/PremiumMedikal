using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class SmilarProductDto
    {
        public string Name { get; set; }
        public string SingleProductHref { get; set; }
        public string MainPhotoFileName{ get; set; }
        public string PhotoAltTag{ get; set; }
        public bool HasNewBadge{ get; set; }
    }
}
