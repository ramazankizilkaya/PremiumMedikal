using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Dtos.ComponentDtos
{
    public class ProductPhotoDto
    {
        public int Id { get; set; }
        public string PhotoFileName { get; set; }
        public bool IsMainPhoto { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
