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
    public class AdminProductDto
    {
        public int Id { get; set; }
        public string MainPhotoFileName { get; set; }
        public string PhotoAltTagTR { get; set; }
        public string NameTR { get; set; }
        public string BrandName { get; set; }
        public string TopCategoryName { get; set; }
        public string MiddleCategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public ProductOfferType ProductOfferType { get; set; }

    }
}
