using AutoMapper;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedikalMarket.UI.Components
{
    public class SimilarProducts : ViewComponent
    {
        private readonly IProductRepository _proRepo;
        private readonly IMapper _mapper;
        private readonly IErrorLogRepository _errorRepo;

        public SimilarProducts(IProductRepository proRepo, IMapper mapper, IErrorLogRepository errorRepo)
        {
            _proRepo = proRepo;
            _mapper = mapper;
            _errorRepo = errorRepo;
        }

        public IViewComponentResult Invoke(int proId)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                ViewBag.Title = culture.Equals("tr") ? "Benzer Ürünler" : culture.Equals("ru") ? "подобная продукция" : "Sımılar Products";
                ViewBag.newBadge = culture.Equals("tr") ? "Yeni" : culture.Equals("ru") ? "Новый" : "New";
                ViewBag.freeShipping = culture.Equals("tr") ? "Ücretsiz Kargo" : culture.Equals("ru") ? "бесплатная доставка" : "Free Shipping";

                var proDb = _proRepo.GetProductWithId(proId);
                List<Product> productsDb = new List<Product>();
                List<SmilarProductDto> dtoList = new List<SmilarProductDto>();

                if (proDb.SubCategoryId != null)
                {
                    var simprosWithSameSubcate = _proRepo.GetProductsWithSubcateId((int)proDb.SubCategoryId).ToList();
                    simprosWithSameSubcate.Remove(proDb);
                    simprosWithSameSubcate.ShuffleMyList();
                    productsDb.AddRange(simprosWithSameSubcate.Take(10));
                }

                if (proDb.MiddleCategoryId != null && productsDb.Count < 10)
                {
                    var simprosWithSameMidcate = _proRepo.GetProductsWithMidcateId((int)proDb.MiddleCategoryId).ToList();
                    simprosWithSameMidcate.Remove(proDb);
                    simprosWithSameMidcate.ShuffleMyList();
                    productsDb.AddRange(simprosWithSameMidcate.Take(10 - productsDb.Count));
                }

                if (productsDb.Count < 10)
                {
                    var simprosWithSameTopcate = _proRepo.GetProductsWithTopcateId(proDb.TopCategoryId).ToList();
                    simprosWithSameTopcate.Remove(proDb);
                    simprosWithSameTopcate.ShuffleMyList();
                    productsDb.AddRange(simprosWithSameTopcate.Take(10 - productsDb.Count));
                }

                productsDb.ShuffleMyList();
                var myDistinctList = productsDb.GroupBy(i => i.Id)
                .Select(g => g.First()).ToList();
                

                if (culture.Equals("tr"))
                {
                    foreach (var item in myDistinctList)
                    {
                        SmilarProductDto dto = new SmilarProductDto();

                        dto.MainPhotoFileName = item.MainPhotoFileName;
                        dto.PhotoAltTag = item.PhotoAltTagTR;
                        dto.HasNewBadge = item.HasNewBadge;
                        dto.Name = item.NameTR;
                        dto.SingleProductHref = item.MiddleCategory == null ? $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.ProductNameUrlTR}" : item.SubCategory == null ? $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.ProductNameUrlTR}" : $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.SubCategory.SubCategoryNameUrlTR}/{item.ProductNameUrlTR}";
                        dtoList.Add(dto);
                    }
                }

                else if (culture.Equals("ru"))
                {
                    foreach (var item in myDistinctList)
                    {
                        SmilarProductDto dto = new SmilarProductDto();

                        dto.MainPhotoFileName = item.MainPhotoFileName;
                        dto.PhotoAltTag = item.PhotoAltTagRU;
                        dto.HasNewBadge = item.HasNewBadge;
                        dto.Name = item.NameRU;
                        dto.SingleProductHref = item.MiddleCategory == null ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.ProductNameUrlRU}" : item.SubCategory == null ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.ProductNameUrlRU}" : $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.SubCategory.SubCategoryNameUrlRU}/{item.ProductNameUrlRU}";
                        dtoList.Add(dto);
                    }
                }

                else
                {
                    foreach (var item in myDistinctList)
                    {
                        SmilarProductDto dto = new SmilarProductDto();

                        dto.MainPhotoFileName = item.MainPhotoFileName;
                        dto.PhotoAltTag = item.PhotoAltTagEN;
                        dto.HasNewBadge = item.HasNewBadge;
                        dto.Name = item.NameEN;
                        dto.SingleProductHref = item.MiddleCategory == null ? $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.ProductNameUrlEN}" : item.SubCategory == null ? $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.ProductNameUrlEN}" : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.ProductNameUrlEN}";
                        dtoList.Add(dto);
                    }
                }

                return View(dtoList);
            }

            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "SimilarProductsComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
        }
    }
}
