using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Areas.Admin.Dtos
{
    public class AdminEditProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen Türkçe ürün adını giriniz.")]
        public string NameTR { get; set; }

        [Required(ErrorMessage = "Lütfen İngilizce ürün adını giriniz.")]
        public string NameEN { get; set; }

        [Required(ErrorMessage = "Lütfen Rusça ürün adını giriniz.")]
        public string NameRU { get; set; }

        [Required(ErrorMessage = "Lütfen Türkçe ürün açıklamasını giriniz.")]
        public string ProductDescriptionTR { get; set; }

        [Required(ErrorMessage = "Lütfen İngilizce ürün açıklamasını giriniz.")]
        public string ProductDescriptionEN { get; set; }

        [Required(ErrorMessage = "Lütfen Rusça ürün açıklamasını giriniz.")]
        public string ProductDescriptionRU { get; set; }

        public string MainPhotoFileName { get; set; }
        public string ProductCode { get; set; }
        public int? DiscountRate { get; set; }
        public int? StockNumber { get; set; }
        public int? NumberInStock { get; set; }

        public ProductOfferType ProductOfferType { get; set; }

        [Required(ErrorMessage = "Lütfen ürün reklam tipini seçiniz. Herhangi bir reklam yoksa \"Normal Ürün\" seçeneğini seçiniz.")]
        public int SelectedProductOfferType { get; set; }
        public bool HasNewBadge { get; set; }
        public bool IsFreeShipping { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan Türkçe meta title açıklamasını giriniz.")]
        public string HeadTitleTR { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan İngilizce meta title açıklamasını giriniz.")]
        public string HeadTitleEN { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan Rusça meta title açıklamasını giriniz.")]
        public string HeadTitleRU { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan Türkçe meta description açıklamasını giriniz.")]
        public string HeadDescriptionTR { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan İngilizce meta description açıklamasını giriniz.")]
        public string HeadDescriptionEN { get; set; }

        [Required(ErrorMessage = "Lütfen ürün detay sayfasında görünecek olan Rusça meta description açıklamasını giriniz.")]
        public string HeadDescriptionRU { get; set; }

        [Required(ErrorMessage = "Lütfen ürün fotoğrafının Türkçe alt tag açıklamasını giriniz.")]
        public string PhotoAltTagTR { get; set; }

        [Required(ErrorMessage = "Lütfen ürün fotoğrafının İngilizce alt tag açıklamasını giriniz.")]
        public string PhotoAltTagEN { get; set; }

        [Required(ErrorMessage = "Lütfen ürün fotoğrafının Rusça alt tag açıklamasını giriniz.")]
        public string PhotoAltTagRU { get; set; }

        [Required(ErrorMessage = "Lütfen marka seçiniz.")]
        public int SelectedBrandId { get; set; }
        public List<Brand> Brands { get; set; }

        [Required(ErrorMessage = "Lütfen ana kategori seçiniz.")]
        public int SelectedTopCategoryId { get; set; }
        public List<TopCategory> TopCategories { get; set; }

        public int? SelectedMiddleCategoryId { get; set; }
        public List<MiddleCategory> MiddleCategories { get; set; }

        public int? SelectedSubCategoryId { get; set; }
        public List<SubCategory> SubCategories { get; set; }

        public IFormFile MainPhoto { get; set; }
        public List<IFormFile> AdditionalPhotos { get; set; }

        public List<AdminProductPhotoDto> AdminProductPhotoDtos { get; set; }
    }
}
