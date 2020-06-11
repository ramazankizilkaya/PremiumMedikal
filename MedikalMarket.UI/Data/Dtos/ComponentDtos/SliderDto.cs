using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class SliderDto
    {
        public string Culture { get; set; }
        public string PhotoFileName { get; set; }
        public string ThumbFileName { get; set; }
        public string PhotoAltTag { get; set; }
        public string SliderHref { get; set; }
        public SliderTargetType SliderTargetType { get; set; }
    }
}
