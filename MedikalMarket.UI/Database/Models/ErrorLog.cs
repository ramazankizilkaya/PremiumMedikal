using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Database.Models
{
    public class ErrorLog:BaseModel
    {
        public string ErrorDetail { get; set; }
        public string ErrorLocation { get; set; }
        public string Culture { get; set; }
        public string ErrorUrl { get; set; }
        public int StatusCode { get; set; }
    }
}
