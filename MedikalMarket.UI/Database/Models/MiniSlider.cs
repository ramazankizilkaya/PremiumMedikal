using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class MiniSlider : BaseModel
    {
        public string Culture { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string SliderHref { get; set; }
        public int TargetProductId { get; set; }
        public string TargetProductName { get; set; }
    }
}
