using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class HomePageAds:BaseModel
    {
        public string Culture { get; set; }
        public string PhotoFileName { get; set; }
        public string AltTag { get; set; }
        public string SliderHref { get; set; }
        public SliderTargetType SliderTargetType { get; set; }
    }
}
