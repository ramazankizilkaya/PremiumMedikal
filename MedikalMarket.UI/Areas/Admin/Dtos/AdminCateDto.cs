using MedikalMarket.UI.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminCateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        [MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string NameTR { get; set; }

        [Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        [MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string NameEN { get; set; }

        [Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        [MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string NameRU { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string TopCategoryNameUrlTR { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string TopCategoryNameUrlRU { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string TopCategoryNameUrlEN { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadTitleTR { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadTitleEN { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadTitleRU { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadDescriptionTR { get; set; }

        //[Required(ErrorMessage = "Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadDescriptionEN { get; set; }

        //[Required(ErrorMessage ="Lütfen bu alanı doldurunuz")]
        //[MaxLength(200, ErrorMessage = "En Fazla 200 karakter girilebilir.")]
        public string HeadDescriptionRU { get; set; }

        public int TopCategoryId { get; set; }
        public int MiddleCategoryId { get; set; }
        public CategoryType CategoryType { get; set; }
        
    }
}
