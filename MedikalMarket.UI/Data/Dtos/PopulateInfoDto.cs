using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class PopulateInfoDto
    {
        public PopulateInfoDto()
        {
            Counts = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Counts { get; set; }
    }
}
