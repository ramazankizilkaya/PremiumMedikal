using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class Slider:BaseModel
    {
        public string Culture { get; set; }
        public string PhotoFileName { get; set; }
        public string ThumbFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string SliderHref { get; set; }
        public SliderTargetType SliderTargetType { get; set; }

        public int? TargetTopCategoryId { get; set; }
        public int? TargetMiddleCategoryId { get; set; }
        public int? TargetSubCategoryId { get; set; }
        public int? TargetProductId { get; set; }
        public int? TargetBrandId { get; set; }
    }
}
