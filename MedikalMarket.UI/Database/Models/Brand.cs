using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class Brand:BaseModel
    {
        public string BrandName { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoAltTagTR { get; set; }
        public string PhotoAltTagEN { get; set; }
        public string BandNameUrl { get; set; }
        public string PhotoAltTagRU { get; set; }

        public string MasterPageMetaTitleTR { get; set; }
        public string MasterPageMetaTitleEN { get; set; }
        public string MasterPageMetaTitleRU { get; set; }
        public string MasterPageMetaDescriptionTR { get; set; }
        public string MasterPageMetaDescriptionEN { get; set; }
        public string MasterPageMetaDescriptionRU { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
