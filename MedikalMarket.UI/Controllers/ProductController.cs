using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MedikalMarket.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFavoriteProductRepository _fpRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IStringLocalizer<ProductController> _localizer;

        public ProductController(ITopCategoryRepository topRepo, IProductRepository productRepo, IErrorLogRepository errorRepo, IHttpContextAccessor httpContextAccessor, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IStringLocalizer<ProductController> localizer, IFavoriteProductRepository fpRepo, ICustomerRepository customerRepo, IBrandRepository brandRepo)
        {
            _topRepo = topRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _httpContextAccessor = httpContextAccessor;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _localizer = localizer;
            _fpRepo = fpRepo;
            _customerRepo = customerRepo;
            _brandRepo = brandRepo;
        }

        #region Single Product
        [Route("/tr/urun-detay/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/tr/urun-detay/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/tr/urun-detay/{BandNameUrl}/{TopCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/en/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/en/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/en/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/ru/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/ru/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{ProductNameUrl}")]
        [Route("/ru/product-detail/{BandNameUrl}/{TopCategoryNameUrl}/{ProductNameUrl}")]
        public IActionResult SingleProduct([FromRoute] string ProductNameUrl)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            string comingUrl1 = ProductNameUrl;
            string x = Request.Headers["Referer"];
            var url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();

            string[] urlParts2 = x.Split("/");
            string[] urlParts3 = url.Split("/");

            string comingUrl2 = urlParts2[urlParts2.Length - 1];
            string comingUrl3 = urlParts3[urlParts3.Length - 1];

            string finalProNameUrl = !comingUrl1.Equals("placeholder.gif") ? comingUrl1 : !comingUrl2.Equals("placeholder.gif") ? comingUrl2 : comingUrl3;

            try
            {
                var product = _productRepo.GetProductWithNameUrl(finalProNameUrl, "tr") ?? _productRepo.GetProductWithNameUrl(finalProNameUrl, "en");
                if (product == null)
                {
                    HttpContext.Session.SetString("alert", _localizer["Ürün bulunamadı."]);
                    return LocalRedirect("/");
                }
                ViewBag.metaTitle = culture.Equals("tr") ? product.HeadTitleTR : culture.Equals("ru") ? product.HeadTitleRU : product.HeadTitleEN;
                ViewBag.metaDesc= culture.Equals("tr") ? product.HeadDescriptionTR : culture.Equals("ru") ? product.HeadDescriptionRU : product.HeadDescriptionEN;

                bool favPros = false;

                if (HttpContext.Session.GetObject<SessionDto>("customerInfo") != null)
                {
                    var customer = _customerRepo.GetEntityById(HttpContext.Session.GetObject<SessionDto>("customerInfo").Id);
                    favPros = _fpRepo.AnyEntity(x => x.CustomerId == customer.Id && x.ProductId == product.Id);
                }

                SingleProductDto dto = new SingleProductDto();
                dto.Id = product.Id;
                dto.Name = culture.Equals("tr") ? product.NameTR : culture.Equals("ru") ? product.NameRU : product.NameEN;
                dto.ProductDescription = culture.Equals("tr") ? product.ProductDescriptionTR : culture.Equals("ru") ? product.ProductDescriptionRU : product.ProductDescriptionEN;
                dto.MainPhotoFileName = product.MainPhotoFileName;
                dto.PhotoAltTag = culture.Equals("tr") ? product.PhotoAltTagTR : culture.Equals("ru") ? product.PhotoAltTagRU : product.PhotoAltTagEN;
                dto.ProductCode = product.ProductCode;
                dto.DiscountRate = product.DiscountRate;
                dto.HasNewBadge = product.HasNewBadge;
                dto.IsFreeShipping = product.IsFreeShipping;
                dto.HeadTitle = culture.Equals("tr") ? product.HeadTitleTR : culture.Equals("ru") ? product.HeadTitleRU : product.HeadTitleEN;
                dto.HeadDescription = culture.Equals("tr") ? product.HeadDescriptionTR : culture.Equals("ru") ? product.HeadDescriptionRU : product.HeadDescriptionEN;
                dto.BrandName = product.Brand.BrandName;
                dto.TopCategoryName = culture.Equals("tr") ? product.TopCategory.NameTR : culture.Equals("ru") ? product.TopCategory.NameRU : product.TopCategory.NameEN;
                dto.TopCategoryNameUrl = culture.Equals("tr") ? product.TopCategory.TopCategoryNameUrlTR : product.TopCategory.TopCategoryNameUrlEN;
                dto.MiddleCategoryName = culture.Equals("tr") ? product.MiddleCategory?.NameTR : culture.Equals("ru") ? product.MiddleCategory?.NameRU : product.MiddleCategory?.NameEN;
                dto.MiddleCategoryNameUrl = culture.Equals("tr") ? product.MiddleCategory?.MiddleCategoryNameUrlTR : product.MiddleCategory?.MiddleCategoryNameUrlEN;
                dto.SubCategoryName = culture.Equals("tr") ? product.SubCategory?.NameTR : culture.Equals("ru") ? product.SubCategory?.NameRU : product.SubCategory?.NameEN;
                dto.SubCategoryNameUrl = culture.Equals("tr") ? product.SubCategory?.SubCategoryNameUrlTR : product.SubCategory?.SubCategoryNameUrlEN;
                dto.ProductOfferType = product.ProductOfferType;
                dto.IsFavorite = favPros;
                dto.PhotoAltTag = culture.Equals("tr") ? product.PhotoAltTagTR : culture.Equals("ru") ? product.PhotoAltTagRU : product.PhotoAltTagEN;

                foreach (var item in product.ProductPhotos.Where(x=>!x.IsMainPhoto))
                {
                    SingleProductPhotoDto photoDto = new SingleProductPhotoDto();
                    photoDto.Id = item.Id;
                    photoDto.PhotoFileName = item.PhotoFileName;
                    dto.ProductPhotoDtos.Add(photoDto);
                }
                return View(dto);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "SingleProduct",
                    ErrorDetail = e.Message + " , " + e.InnerException + $"comingUrl1:{comingUrl1}, comingUrl2:{comingUrl2}, comingUrl3:{comingUrl3}, finalProNameUrl:{finalProNameUrl}",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
        }

        #endregion

        #region By TopCate
        [Route("/tr/kategoriler/{TopCategoryNameUrl}/sayfa/{page}")]
        [Route("/en/categories/{TopCategoryNameUrl}/page/{page}")]
        [Route("/ru/categories/{TopCategoryNameUrl}/page/{page}")]
        public IActionResult ProductsByTopCategory(string topCategoryNameUrl, int? page)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                int currentPage = page ?? 1;
                TopCategory topCate = _topRepo.GetTopCateByUrlTR(topCategoryNameUrl) ?? _topRepo.GetTopCateByUrlEN(topCategoryNameUrl);

                ViewBag.metaTitle = culture.Equals("tr") ? topCate.HeadTitleTR : culture.Equals("ru") ? topCate.HeadTitleRU : topCate.HeadTitleEN;
                ViewBag.metaDesc = culture.Equals("tr") ? topCate.HeadDescriptionTR : culture.Equals("ru") ? topCate.HeadDescriptionRU : topCate.HeadDescriptionEN;

                int productCount = _productRepo.CountEntity(x=>x.TopCategoryId== topCate.Id);
                if (productCount==0)
                {
                    HttpContext.Session.SetString("alert", _localizer["Bu kategoride ürün bulunmamaktadır."]);
                    return LocalRedirect("/");
                }
                double division = Convert.ToDouble(productCount) / Convert.ToDouble(30);
                int lastPage = Convert.ToInt32(Math.Ceiling(division));
                if (currentPage > lastPage || currentPage < 0)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        ErrorLocation = "ProductsByTopCategory",
                        Culture = culture,
                        ErrorDetail = $"Gitmek istenilen sayfa numarası hatalı. Hatalı sayfa numarası: {currentPage}",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                var sortVal = HttpContext.Session.GetString("categorySort");
                var products = _productRepo.GetProductsWithTopcateId(topCate.Id).AsQueryable();
                if (sortVal!=null)
                {
                    if (sortVal.Equals("0"))
                    {
                        ViewBag.selectedDefault = "selected";
                    }
                    else if(sortVal.Equals("1"))
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
                        ErrorLocation = "ProductsByTopCategory",
                        Culture= culture,
                        ErrorDetail = "targetProducts null döndü",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                //sidebar component için
                ViewBag.cateId = topCate.Id;
                ViewBag.topCategoryName = culture.Equals("tr") ? topCate.NameTR : culture.Equals("ru") ? topCate.NameRU : topCate.NameEN;
                ViewBag.topCategoryNameUrl = culture.Equals("tr") ? topCate.TopCategoryNameUrlTR : topCate.TopCategoryNameUrlEN;

                ViewBag.currentPage = currentPage;
                ViewBag.paginationCount = lastPage;

                List<ProductDto> proDtos = new List<ProductDto>();
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
                    if(currentPage + i <= lastPage) {
                    pageNumbers1.Add(currentPage+i);
                    }
                    //else if (currentPage + i > lastPage && pageNumbers1.Count < 10)
                    //{
                    //    pageNumbers1.Sort();
                    //    pageNumbers1.Add(pageNumbers1[0]-j);
                    //    j++;
                    //}

                }
                pageNumbers1.Sort();

                ViewBag.numbers = pageNumbers1;

                return View(proDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "ProductsByTopCategory",
                    Culture=culture,
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
        }
        #endregion

        #region ByMiddleCate
        [Route("/tr/kategoriler/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/sayfa/{page}")]
        [Route("/en/categories/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/page/{page}")]
        [Route("/ru/categories/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/page/{page}")]
        public IActionResult ProductsByMiddleCategory(string topCategoryNameUrl, string middleCategoryNameUrl, int? page)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();
            try
            {
                int currentPage = page ?? 1;
                TopCategory topCate = _topRepo.GetTopCateByUrlTR(topCategoryNameUrl) ?? _topRepo.GetTopCateByUrlEN(topCategoryNameUrl);
                MiddleCategory midCate = _midRepo.GetMidCateByUrlTR(middleCategoryNameUrl, topCate.Id) ?? _midRepo.GetMidCateByUrlEN(middleCategoryNameUrl, topCate.Id);

                ViewBag.metaTitle = culture.Equals("tr") ? midCate.HeadTitleTR : culture.Equals("ru") ? midCate.HeadTitleRU : midCate.HeadTitleEN;
                ViewBag.metaDesc = culture.Equals("tr") ? midCate.HeadDescriptionTR : culture.Equals("ru") ? midCate.HeadDescriptionRU : midCate.HeadDescriptionEN;

                int productCount = _productRepo.CountEntity(x => x.TopCategoryId == topCate.Id && x.MiddleCategoryId== midCate.Id);
                if (productCount == 0)
                {
                    HttpContext.Session.SetString("alert", _localizer["Bu kategoride ürün bulunmamaktadır."]);
                    return LocalRedirect("/");
                }
                double division = Convert.ToDouble(productCount) / Convert.ToDouble(30);
                int lastPage = Convert.ToInt32(Math.Ceiling(division));
                if (currentPage > lastPage || currentPage < 0)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        ErrorLocation = "ProductsByMiddleCategory",
                        Culture = culture,
                        ErrorDetail = $"Gitmek istenilen sayfa numarası hatalı. Hatalı sayfa numarası: {currentPage}",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                var products = _productRepo.GetProductsWithMidcateId(midCate.Id).AsQueryable();
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
                        ErrorLocation = "ProductsByTopCategory",
                        Culture = culture,
                        ErrorDetail = "targetProducts null döndü",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                ViewBag.topcategoryName = culture.Equals("tr") ? midCate.TopCategory.NameTR : culture.Equals("ru") ? midCate.TopCategory.NameRU : midCate.TopCategory.NameEN;

                ViewBag.topcategoryNameUrl = culture.Equals("tr") ? midCate.TopCategory.TopCategoryNameUrlTR : midCate.TopCategory.TopCategoryNameUrlEN;

                //category sidebar component için
                ViewBag.cateId = midCate.Id;

                ViewBag.midcategoryName = culture.Equals("tr") ? midCate.NameTR : culture.Equals("ru") ? midCate.NameRU : midCate.NameEN;

                ViewBag.midcategoryNameUrl = culture.Equals("tr") ? midCate.MiddleCategoryNameUrlTR : midCate.MiddleCategoryNameUrlEN;

                ViewBag.currentPage = currentPage;
                ViewBag.paginationCount = lastPage;

                List<ProductDto> proDtos = new List<ProductDto>();
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

                    if (item.SubCategoryId != null)
                    {
                        dto.ProductHref = culture.Equals("tr") ? 
                            $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.SubCategory.SubCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}" 
                            : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.SubCategory.SubCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}" 
                            : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}";
                    }
                    else
                    {
                        dto.ProductHref = culture.Equals("tr") ? 
                            $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}"
                            : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}" 
                            : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}"; 
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
                    //else if (currentPage + i > lastPage && pageNumbers1.Count < 10)
                    //{
                    //    pageNumbers1.Sort();
                    //    pageNumbers1.Add(pageNumbers1[0] - j);
                    //    j++;
                    //}

                }
                pageNumbers1.Sort();

                ViewBag.numbers = pageNumbers1;

                return View(proDtos);

            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "ProductsByMiddleCategory",
                    Culture=culture,
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
        }

        #endregion

        #region BySubCate

        [Route("/tr/kategoriler/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/sayfa/{page}")]
        [Route("/en/categories/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/page/{page}")]
        [Route("/ru/categories/{TopCategoryNameUrl}/{MiddleCategoryNameUrl}/{SubCategoryNameUrl}/page/{page}")]
        public IActionResult ProductsBySubCategory(string topCategoryNameUrl, string middleCategoryNameUrl, string subCategoryNameUrl, int? page)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                int currentPage = page ?? 1;
                TopCategory topCate = _topRepo.GetTopCateByUrlTR(topCategoryNameUrl) ?? _topRepo.GetTopCateByUrlEN(topCategoryNameUrl);

                MiddleCategory midCate = _midRepo.GetMidCateByUrlTR(middleCategoryNameUrl, topCate.Id) ?? _midRepo.GetMidCateByUrlEN(middleCategoryNameUrl, topCate.Id);

                SubCategory subCate = _subRepo.GetSubCateByUrlTR(subCategoryNameUrl, midCate.Id, topCate.Id) ?? _subRepo.GetSubCateByUrlEN(subCategoryNameUrl, midCate.Id, topCate.Id);

                ViewBag.metaTitle = culture.Equals("tr") ? subCate.HeadTitleTR : culture.Equals("ru") ? subCate.HeadTitleRU : subCate.HeadTitleEN;
                ViewBag.metaDesc = culture.Equals("tr") ? subCate.HeadDescriptionTR : culture.Equals("ru") ? subCate.HeadDescriptionRU : subCate.HeadDescriptionEN;

                int productCount = _productRepo.CountEntity(x => x.TopCategoryId == topCate.Id && x.MiddleCategoryId == midCate.Id && x.SubCategoryId== subCate.Id);
                if (productCount == 0)
                {
                    HttpContext.Session.SetString("alert", _localizer["Bu kategoride ürün bulunmamaktadır."]);
                    return LocalRedirect("/");
                }

                double division = Convert.ToDouble(productCount) / Convert.ToDouble(30);
                int lastPage = Convert.ToInt32(Math.Ceiling(division));

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

                var products = _productRepo.GetProductsWithSubcateId(subCate.Id).AsQueryable();
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
                        ErrorLocation = "ProductsBySubCategory",
                        Culture = culture,
                        ErrorDetail = "targetProducts null döndü",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return LocalRedirect("/Error/500");
                }

                ViewBag.topcategoryName = culture.Equals("tr") ? midCate.TopCategory.NameTR : culture.Equals("ru") ? midCate.TopCategory.NameRU : midCate.TopCategory.NameEN;

                ViewBag.topcategoryNameUrl = culture.Equals("tr") ? midCate.TopCategory.TopCategoryNameUrlTR : midCate.TopCategory.TopCategoryNameUrlEN;

                ViewBag.midcategoryName = culture.Equals("tr") ? midCate.NameTR : culture.Equals("ru") ? midCate.NameRU : midCate.NameEN;

                ViewBag.midcategoryNameUrl = culture.Equals("tr") ? midCate.MiddleCategoryNameUrlTR : midCate.MiddleCategoryNameUrlEN;

                //category sidebar componenti
                ViewBag.cateId = subCate.Id;
                ViewBag.subcategoryName = culture.Equals("tr") ? subCate.NameTR : culture.Equals("ru") ? subCate.NameRU : subCate.NameEN;
                ViewBag.subcategoryNameUrl = subCate.SubCategoryNameUrlTR;

                ViewBag.currentPage = currentPage;
                ViewBag.paginationCount = lastPage;

                List<ProductDto> proDtos = new List<ProductDto>();

                foreach (var item in products)
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

                    dto.ProductHref = culture.Equals("tr") ?
                            $"/tr/urun-detay/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlTR}/{item.MiddleCategory.MiddleCategoryNameUrlTR}/{item.SubCategory.SubCategoryNameUrlTR}/{item.NameTR.ConvertToFriendlyUrl()}"
                            : culture.Equals("ru") ? $"/ru/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlRU}/{item.MiddleCategory.MiddleCategoryNameUrlRU}/{item.SubCategory.SubCategoryNameUrlRU}/{item.NameEN.ConvertToFriendlyUrl()}"
                            : $"/en/product-detail/{item.Brand.BandNameUrl}/{item.TopCategory.TopCategoryNameUrlEN}/{item.MiddleCategory.MiddleCategoryNameUrlEN}/{item.SubCategory.SubCategoryNameUrlEN}/{item.NameEN.ConvertToFriendlyUrl()}";

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
                    ErrorLocation = "ProductsBySubCategory",
                    Culture=culture,
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
        }
        #endregion

        [HttpPost]
        public JsonResult AddRemoveToFavorites(int productId)
        {
            bool a = _fpRepo.AnyEntity(x => x.ProductId == productId && x.CustomerId == HttpContext.Session.GetObject<SessionDto>("customerInfo").Id);

            if (a)
            {
                bool b=_fpRepo.HardDeleteEntity(_fpRepo.FindEntity(x => x.ProductId == productId && x.CustomerId == HttpContext.Session.GetObject<SessionDto>("customerInfo").Id));

                if (b)
                {
                    string rspText = _localizer["Ürün favori listenizden silinmiştir."];
                    string title = _localizer["Başarılı"];
                    return Json(new { success = true, responseText = rspText, responseTitle = title, typO = "delete" });
                }
                else
                {
                    string rspText = _localizer["Ürün favori listenizden silinirken hata oluştu. Lütfen tekrar deneyiniz."];
                    string title = _localizer["Hata"];
                    return Json(new { success = false, responseText = rspText, responseTitle = title, typO = "delete" });
                }
            }

           bool c = _fpRepo.CreateEntity(new FavoriteProduct
            {
                CustomerId = HttpContext.Session.GetObject<SessionDto>("customerInfo").Id,
                ProductId = productId
            });
            if (c)
            {
                string rspText = _localizer["Ürün favori listenize eklenmiştir."];
                string title= _localizer["Başarılı"];
                return Json(new { success = true, responseText = rspText, responseTitle = title, typO = "create" });
            }
            else
            {
                string title = _localizer["Hata"];
                string rspText = _localizer["Ürün favori listenize eklenirken hata oluştu."];
                return Json(new { success = false, responseText = rspText, responseTitle= title, typO = "create" });
            }
        }


        [HttpPost]
        public JsonResult DeleteFavoriteProduct(int fpId)
        {
            try
            {
                var favPro = _fpRepo.GetEntityById(fpId) ?? null;

                bool a = _fpRepo.HardDeleteEntity(favPro);

                if (a)
                {
                    string title = _localizer["Başarılı"];
                    string rspText = _localizer["Ürün favori listenizden çıkarıldı."];

                    return Json(new { success = true, responseText = rspText, responseTitle = title});
                }
                else
                {
                    string title = _localizer["Uyarı"];
                    string rspText = _localizer["Ürün favori listenizden çıkarılamadı."];
                    return Json(new { success = false, responseText = rspText, responseTitle = title });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "DeleteFavoriteProduct",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                string title = _localizer["Hata"];
                string rspText = _localizer["Hata oluştu ve kayıt altına alındı."];
                return Json(new { success = false, responseText = rspText, responseTitle = title });
            }
            
        }

        [HttpGet("search")]
        public JsonResult SearchResults()
        {
            try
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                string culture = locale.RequestCulture.UICulture.ToString();
                List<string> searchItems = new List<string>();
                string term = HttpContext.Request.Query["term"].ToString();

                var topCateNames = culture.Equals("tr") ? _topRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _topRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _topRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var midCateNames = culture.Equals("tr") ? _midRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _midRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _midRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var subCateNames = culture.Equals("tr") ? _subRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _subRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _subRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var brandNames = _brandRepo.FindEntities(x => !x.IsDeleted && x.BrandName.Contains(term)).Select(x => x.BrandName).ToList();

                var products = culture.Equals("tr") ? _productRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _productRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _productRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                searchItems.AddRange(topCateNames);
                searchItems.AddRange(midCateNames);
                searchItems.AddRange(subCateNames);
                searchItems.AddRange(brandNames);
                searchItems.AddRange(products);

                return Json(searchItems);
            }
            catch
            {
                return Json("error");
            }

        }

        public JsonResult ChangeSort(string sortingen)
        {
            HttpContext.Session.SetString("categorySort", sortingen);
            return Json("ok");
        }
    }
}