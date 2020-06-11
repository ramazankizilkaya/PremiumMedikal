using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos
{
    public class SingleProductPhotoDto
    {
        public int Id { get; set; }
        public string PhotoFileName { get; set; }
        public string AltTag { get; set; }
        public bool IsMainPhoto { get; set; }
    }
}
