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
    public class ProductTabs : ViewComponent
    {
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;

        public ProductTabs(IProductRepository productRepo, IErrorLogRepository errorRepo)
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
                var proTabsDto = new ProductTabsDto();
                var campaignProducts = _productRepo.GetProForSliders(ProductOfferType.CampaignProduct).ToList();
                var onSaleProducts = _productRepo.GetProForSliders(ProductOfferType.OnSaleProduct).ToList();
                var weeklyDealProducts = _productRepo.GetProForSliders(ProductOfferType.WeeklyDealProduct).ToList();
                var newProducts = _productRepo.GetProForSliders(ProductOfferType.NewProduct).OrderByDescending(x => x.CreatedDate).OrderByDescending(x => x.UpdatedDate).ToList();

                campaignProducts.ShuffleMyList();
                onSaleProducts.ShuffleMyList();
                weeklyDealProducts.ShuffleMyList();
                newProducts.ShuffleMyList();
                ViewBag.cul = culture;
                switch (culture)
                {
                    case "tr":
                        proTabsDto.CampaignProductsTitle = "KAMPANYALI ÜRÜNLER";
                        proTabsDto.OnSaleProductsTitle = "İNDİRİMDEKİLER";
                        proTabsDto.WeeklyDealProductsTitle = "HAFTANIN ÜRÜNLERİ";
                        proTabsDto.NewProductsTitle = "YENİ ÜRÜNLER";
                        proTabsDto.FreeShippingText = "Ücretsiz Kargo";
                        proTabsDto.NewBadgeText = "Yeni";

                        foreach (var item in campaignProducts.Take(8))
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
                            proTabsDto.CampaignProducts.Add(dto);
                        }
                        foreach (var item in onSaleProducts.Take(8))
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
                            proTabsDto.OnSaleProducts.Add(dto);
                        }
                        foreach (var item in weeklyDealProducts.Take(8))
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
                            proTabsDto.WeeklyDealProducts.Add(dto);
                        }
                        foreach (var item in newProducts.Take(8))
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
                            proTabsDto.NewProducts.Add(dto);
                        }
                        break;
                    case "en":
                        proTabsDto.CampaignProductsTitle = "CAMPAIGN PRODUCTS";
                        proTabsDto.OnSaleProductsTitle = "DISCOUNTED";
                        proTabsDto.WeeklyDealProductsTitle = "PRODUCTS OF THE WEEK";
                        proTabsDto.NewProductsTitle = "NEW PRODUCTS";
                        proTabsDto.FreeShippingText = "Free Shipping";
                        proTabsDto.NewBadgeText = "New";
                        foreach (var item in campaignProducts.Take(8))
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
                            proTabsDto.CampaignProducts.Add(dto);
                        }
                        foreach (var item in onSaleProducts.Take(8))
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
                            proTabsDto.OnSaleProducts.Add(dto);
                        }
                        foreach (var item in weeklyDealProducts.Take(8))
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
                            proTabsDto.WeeklyDealProducts.Add(dto);
                        }
                        foreach (var item in newProducts.Take(8))
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
                            proTabsDto.NewProducts.Add(dto);
                        }
                        break;
                    case "ru":
                        proTabsDto.CampaignProductsTitle = "АГИТАЦИОННУЮ ПРОДУКЦИЮ";
                        proTabsDto.OnSaleProductsTitle = "ДИСКОНТИРОВАННЫЙ";
                        proTabsDto.WeeklyDealProductsTitle = "ПРОДУКТЫ НЕДЕЛИ";
                        proTabsDto.NewProductsTitle = "НОВЫЕ ПРОДУКТЫ";
                        proTabsDto.FreeShippingText = "бесплатная доставка";
                        proTabsDto.NewBadgeText = "новый";
                        foreach (var item in campaignProducts.Take(8))
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
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/ru/brands/{dto.BrandNameUrl}";
                            proTabsDto.CampaignProducts.Add(dto);
                        }
                        foreach (var item in onSaleProducts.Take(8))
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
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/ru/brands/{dto.BrandNameUrl}";
                            proTabsDto.OnSaleProducts.Add(dto);
                        }
                        foreach (var item in weeklyDealProducts.Take(8))
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
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/ru/brands/{dto.BrandNameUrl}";
                            proTabsDto.WeeklyDealProducts.Add(dto);
                        }
                        foreach (var item in newProducts.Take(8))
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
                            dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/ru/product-detail/{dto.BrandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                            dto.BrandHref = $"/ru/brands/{dto.BrandNameUrl}";
                            proTabsDto.NewProducts.Add(dto);
                        }
                        break;
                    default:
                        proTabsDto.CampaignProductsTitle = "CAMPAIGN PRODUCTS";
                        proTabsDto.OnSaleProductsTitle = "DISCOUNTED";
                        proTabsDto.WeeklyDealProductsTitle = "PRODUCTS OF THE WEEK";
                        proTabsDto.NewProductsTitle = "NEW PRODUCTS";
                        proTabsDto.FreeShippingText = "Free Shipping";
                        proTabsDto.NewBadgeText = "New";
                        foreach (var item in campaignProducts.Take(8))
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
                            proTabsDto.CampaignProducts.Add(dto);
                        }
                        foreach (var item in onSaleProducts.Take(8))
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
                            proTabsDto.OnSaleProducts.Add(dto);
                        }
                        foreach (var item in weeklyDealProducts.Take(8))
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
                            proTabsDto.WeeklyDealProducts.Add(dto);
                        }
                        foreach (var item in newProducts.Take(8))
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
                            proTabsDto.NewProducts.Add(dto);
                        }
                        break;
                }

                return View(proTabsDto);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "ProductTabsComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
            
        }
    }
}
