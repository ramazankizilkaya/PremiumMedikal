using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Components
{
    public class ShowcaseProducts : ViewComponent
    {
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;

        public ShowcaseProducts(IProductRepository productRepo, IErrorLogRepository errorRepo)
        {
            _productRepo = productRepo;
            _errorRepo = errorRepo;
        }

        public IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                var pros = _productRepo.GetProForSliders(ProductOfferType.ShowcaseProduct).ToList();
                pros.ShuffleMyList();
                var pro = pros.Take(10);
                var dtoList = new List<ShowcaseProductsDto>();
                var photoDtoList = new List<ProductPhotoDto>();

                switch (culture)
                {
                    case "tr":
                        ViewBag.title = "Vitrin Ürünleri";
                        ViewBag.newBadge = "Yeni";
                        ViewBag.freeShipping = "Ücretsiz Kargo";
                        foreach (var item in pro)
                        {
                            ShowcaseProductsDto dto = new ShowcaseProductsDto();
                            dto.Id = item.Id;
                            dto.Name = item.NameTR;
                            dto.ProductDescription = item.ProductDescriptionTR;
                            dto.MainPhotoFileName = item.MainPhotoFileName;
                            dto.ProductNameUrl = item.ProductNameUrlTR;
                            dto.PhotoAltTag = item.PhotoAltTagTR;
                            dto.BrandName = item.Brand.BrandName;
                            dto.BrandNameUrl = item.Brand.BandNameUrl;
                            dto.HasNewBadge = item.HasNewBadge;
                            dto.IsFreeShipping = item.IsFreeShipping;
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/tr/urun-detay/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.ProductNameUrlTR}" : item.SubCategory == null ? $"/tr/urun-detay/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.ProductNameUrlTR}" : $"/tr/urun-detay/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.SubCategory.SubCategoryNameUrlTR}/{item.ProductNameUrlTR}";
                            dto.BrandHref = $"/tr/markalar/{dto.BrandNameUrl}";
                            dtoList.Add(dto);
                        }
                        break;
                    case "en":
                        ViewBag.title = "Showcase Products";
                        ViewBag.newBadge = "New";
                        ViewBag.freeShipping = "Free Shipping";
                        foreach (var item in pro)
                        {
                            ShowcaseProductsDto dto = new ShowcaseProductsDto();
                            dto.Id = item.Id;
                            dto.Name = item.NameEN;
                            dto.ProductDescription = item.ProductDescriptionEN;
                            dto.MainPhotoFileName = item.MainPhotoFileName;
                            dto.ProductNameUrl = item.ProductNameUrlEN;
                            dto.PhotoAltTag = item.PhotoAltTagEN;
                            dto.BrandName = item.Brand.BrandName;
                            dto.BrandNameUrl = item.Brand.BandNameUrl;
                            dto.HasNewBadge = item.HasNewBadge;
                            dto.IsFreeShipping = item.IsFreeShipping;
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/en/brands/{dto.BrandNameUrl}";
                            dtoList.Add(dto);
                        }
                        break;
                    case "ru":
                        ViewBag.title = "витрина продуктов";
                        ViewBag.newBadge = "новый";
                        ViewBag.freeShipping = "бесплатная доставка";
                        foreach (var item in pro)
                        {
                            ShowcaseProductsDto dto = new ShowcaseProductsDto();
                            dto.Id = item.Id;
                            dto.Name = item.NameRU;
                            dto.ProductDescription = item.ProductDescriptionRU;
                            dto.MainPhotoFileName = item.MainPhotoFileName;
                            dto.ProductNameUrl = item.ProductNameUrlRU;
                            dto.PhotoAltTag = item.PhotoAltTagRU;
                            dto.BrandName = item.Brand.BrandName;
                            dto.BrandNameUrl = item.Brand.BandNameUrl;
                            dto.HasNewBadge = item.HasNewBadge;
                            dto.IsFreeShipping = item.IsFreeShipping;
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.ProductNameUrlRU}" : item.SubCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.ProductNameUrlRU}" : $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.SubCategory.SubCategoryNameUrlRU}/{item.ProductNameUrlRU}";
                            dto.BrandHref = $"/ru/brands/{dto.BrandNameUrl}";
                            dtoList.Add(dto);
                        }
                        break;
                    default:
                        ViewBag.title = "Showcase Products";
                        ViewBag.newBadge = "New";
                        ViewBag.freeShipping = "Free Shipping";
                        foreach (var item in pro)
                        {
                            ShowcaseProductsDto dto = new ShowcaseProductsDto();
                            dto.Id = item.Id;
                            dto.Name = item.NameEN;
                            dto.ProductDescription = item.ProductDescriptionEN;
                            dto.MainPhotoFileName = item.MainPhotoFileName;
                            dto.ProductNameUrl = item.ProductNameUrlEN;
                            dto.PhotoAltTag = item.PhotoAltTagEN;
                            dto.BrandName = item.Brand.BrandName;
                            dto.BrandNameUrl = item.Brand.BandNameUrl;
                            dto.HasNewBadge = item.HasNewBadge;
                            dto.IsFreeShipping = item.IsFreeShipping;
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/en/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/en/brands/{dto.BrandNameUrl}";
                            dtoList.Add(dto);
                        }
                        break;
                }
                return View(dtoList);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "ShowcaseProductsComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
        }
    }
}
