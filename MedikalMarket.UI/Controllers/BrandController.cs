using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedikalMarket.UI.Controllers
{
    public class BrandController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ProductController> _localizer;

        public BrandController(ITopCategoryRepository topRepo, IProductRepository productRepo, IErrorLogRepository errorRepo, IHttpContextAccessor httpContextAccessor, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IStringLocalizer<ProductController> localizer, IBrandRepository brandRepo)
        {
            _topRepo = topRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _httpContextAccessor = httpContextAccessor;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _localizer = localizer;
            _brandRepo = brandRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/tr/markalar/{BrandNameUrl}/sayfa/{page}")]
        [Route("/en/brands/{BrandNameUrl}/page/{page}")]
        [Route("/ru/brands/{BrandNameUrl}/page/{page}")]
        public IActionResult ProductsByBrand(string BrandNameUrl, int? page)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                int currentPage = page ?? 1;

                var brand = _brandRepo.FindEntity(x => x.BrandName.ConvertToFriendlyUrl().Equals(BrandNameUrl));

                int productCount = _productRepo.CountEntity(x => x.BrandId== brand.Id);
                if (productCount == 0)
                {
                    HttpContext.Session.SetString("alert", _localizer["Bu markaya ait ürün bulunmamaktadır."]);
                    return LocalRedirect("/");
                }
                ViewBag.brandName = brand.BrandName;
                ViewBag.brandNameUrl = brand.BrandName.ConvertToFriendlyUrl();
                ViewBag.brandId = brand.Id;

                ViewBag.metaTitle = culture.Equals("tr") ? brand.MasterPageMetaTitleTR : culture.Equals("ru") ? brand.MasterPageMetaTitleRU : brand.MasterPageMetaTitleEN;
                ViewBag.metaDesc = culture.Equals("tr") ? brand.MasterPageMetaDescriptionTR : culture.Equals("ru") ? brand.MasterPageMetaDescriptionRU : brand.MasterPageMetaDescriptionEN;

                double division = Convert.ToDouble(productCount) / Convert.ToDouble(30);
                int lastPage = Convert.ToInt32(Math.Ceiling(division));

                ViewBag.currentPage = currentPage;
                ViewBag.paginationCount = lastPage;

                if (currentPage > lastPage || currentPage < 0)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        ErrorLocation = "ProductsBySubCategory",
                        Culture = culture,
                        ErrorDetail = $"Gitmek istenilen sayfa numarası hatalı. Hatalı sayfa numarası: {currentPage}",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                List<ProductDto> proDtos = new List<ProductDto>();
                var products = _productRepo.GetProductsWithBrandId(brand.Id).AsQueryable();
                var sortVal = HttpContext.Session.GetString("categorySort");
                if (sortVal != null)
                {
                    if (sortVal.Equals("0"))
                    {
                        ViewBag.selectedDefault = "selected";
                    }
                    else if (sortVal.Equals("1"))
                    {
                        products = culture.Equals("tr") ? products.OrderBy(x => x.NameTR) : culture.Equals("ru") ? products.OrderBy(x => x.NameRU) : products.OrderBy(x => x.NameEN);
                        ViewBag.selectedByProductName = "selected";
                    }
                    else if (sortVal.Equals("2"))
                    {
                        products = products.OrderBy(x => x.Brand.BrandName);
                        ViewBag.selectedByBrandName = "selected";
                    }
                    else if (sortVal.Equals("3"))
                    {
                        products = products.OrderByDescending(x => x.CreatedDate);
                        ViewBag.selectedByNew = "selected";
                    }
                    else if (sortVal.Equals("4"))
                    {
                        products = products.OrderByDescending(x => x.DiscountRate);
                        ViewBag.selectedBySale = "selected";
                    }
                }
                var targetProducts =
                            page == 1 ? products.Take(30)
                            : (page > 1 && page < lastPage) ? products.Skip((currentPage - 1) * 30).Take(30)
                            : page == lastPage ? products.Skip((currentPage - 1) * 30).Take(productCount - ((currentPage - 1) * 30)) : null;

                if (targetProducts == null)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        ErrorLocation = "ProductsByBrand",
                        Culture = culture,
                        ErrorDetail = "targetProducts null döndü",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                foreach (var item in targetProducts.ToList())
                {
                    ProductDto dto = new ProductDto();
                    dto.MainPhotoFileName = item.MainPhotoFileName;
                    dto.PhotoAltTag = culture.Equals("tr") ? item.PhotoAltTagTR : culture.Equals("ru") ? item.PhotoAltTagRU : item.PhotoAltTagEN;
                    dto.HasNewBadge = item.HasNewBadge;
                    dto.IsFreeShipping = item.IsFreeShipping;
                    dto.Name = culture.Equals("tr") ? item.NameTR : culture.Equals("ru") ? item.NameRU : item.NameEN;
                    dto.Id = item.Id;
                    dto.ProductNameUrl = culture.Equals("tr") ? item.ProductNameUrlTR : culture.Equals("ru") ? item.ProductNameUrlRU : item.ProductNameUrlEN;
                    dto.BrandName = item.Brand.BrandName;
                    dto.BrandNameUrl = item.Brand.BandNameUrl;

                    if (item.SubCategoryId != null && item.MiddleCategoryId != null)
                    {
                        dto.ProductHref = culture.Equals("tr") ? $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.SubCategory.SubCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}"
                            : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.SubCategory.SubCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}"
                            : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}";
                    }
                    else if (item.SubCategoryId == null && item.MiddleCategoryId == null)
                    {
                        dto.ProductHref = culture.Equals("tr") ? $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}" : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}" : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}";
                    }
                    else
                    {
                        dto.ProductHref = culture.Equals("tr") ? $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}" : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}" : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}";
                    }
                    proDtos.Add(dto);
                }

                //pagination numaralandırması
                List<int> pageNumbers1 = new List<int>();
                for (int i = 1; i <= 5; i++)
                {
                    if (currentPage - i > 0)
                    {
                        pageNumbers1.Add(currentPage - i);
                    }
                }
                int listCount = pageNumbers1.Count;
                for (int i = 0, j = 1; i < 10 - listCount; i++)
                {
                    if (currentPage + i <= lastPage)
                    {
                        pageNumbers1.Add(currentPage + i);
                    }
                }
                pageNumbers1.Sort();
                ViewBag.numbers = pageNumbers1;
                return View(proDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "ProductsByBrand",
                    Culture=culture,
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
        }
    }
}