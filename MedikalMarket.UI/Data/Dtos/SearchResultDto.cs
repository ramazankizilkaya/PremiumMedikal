using MedikalMarket.UI.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class SearchResultDto
    {
        public string Text { get; set; }
        public string TargetLink { get; set; }
        public SearchResultType SearchResultType { get; set; }
    }
}
