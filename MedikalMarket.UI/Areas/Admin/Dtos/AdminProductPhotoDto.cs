using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminProductPhotoDto
    {
        public int Id { get; set; }
        public string PhotoFileName { get; set; }
        public bool IsMainPhoto { get; set; }

    }
}
