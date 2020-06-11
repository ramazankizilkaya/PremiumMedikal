using AutoMapper;
using MedikalMarket.UI.Areas.Admin.Dtos;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdProductController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IAdProductRepository _adproRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public AdProductController(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IProductRepository productRepo, IMapper mapper, IAdProductRepository adproRepo, IBrandRepository brandRepo, IWebHostEnvironment environment)
        {
            _topRepo = topRepo;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _mapper = mapper;
            _adproRepo = adproRepo;
            _brandRepo = brandRepo;
            _environment = environment;
        }

        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return View();
        }

        public IActionResult AdProductList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");

            var adproDtos = new List<AdminAdProductDto>();
            var adprosDb = _adproRepo.FindEntitiesNoTrack(x=>!x.IsDeleted).ToList();
            adprosDb.ForEach(x => adproDtos.Add(_mapper.Map<AdminAdProductDto>(x)));
            adproDtos.ForEach(x => x.GenericTargetName = ReturnGenericName(x.Id));
            return View(adproDtos.OrderByDescending(x => x.Id));
        }

        public string ReturnGenericName(int adproId)
        {
            try
            {
                var sliderDb = _adproRepo.GetEntityById(adproId);
                //Ana
                if (sliderDb.TargetTopCategoryId != null && sliderDb.TargetMiddleCategoryId == null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetBrandId == null && sliderDb.TargetProductId == null)
                {
                    return _topRepo.GetEntityById((int)sliderDb.TargetTopCategoryId).NameTR;
                }
                //Orta
                else if (sliderDb.TargetMiddleCategoryId != null && sliderDb.TargetTopCategoryId != null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetBrandId == null && sliderDb.TargetProductId == null)
                {
                    return _midRepo.GetEntityById((int)sliderDb.TargetMiddleCategoryId).NameTR;
                }
                //Alt
                else if (sliderDb.TargetMiddleCategoryId != null && sliderDb.TargetTopCategoryId != null && sliderDb.TargetSubCategoryId != null && sliderDb.TargetBrandId == null && sliderDb.TargetProductId == null)
                {
                    return _subRepo.GetEntityById((int)sliderDb.TargetSubCategoryId).NameTR;
                }
                //Marka
                else if (sliderDb.TargetBrandId != null && sliderDb.TargetTopCategoryId == null && sliderDb.TargetMiddleCategoryId == null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetProductId == null)
                {
                    return _brandRepo.GetEntityById((int)sliderDb.TargetBrandId).BrandName;
                }
                //Ürün
                else if (sliderDb.TargetProductId != null && sliderDb.TargetBrandId == null && sliderDb.TargetTopCategoryId == null && sliderDb.TargetMiddleCategoryId == null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetBrandId == null)
                {
                    return _productRepo.GetEntityById((int)sliderDb.TargetProductId).NameTR;
                }
                return null;
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message,
                    ErrorLocation = "AdProductController ReturnGenericName",
                    ErrorUrl = HttpContext.Request.Path
                });
                return null;
            }
        }

        public IActionResult CreateAdProduct()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var adproDto = new AdminCreateAdProductDto();
            adproDto.TopCategories = _topRepo.FindEntities(x=>!x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
            return PartialView(adproDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAdProduct(AdminCreateAdProductDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
            try
            {
                if (ModelState.IsValid)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

                    if (dto.AdProductPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.AdProductPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("AdProductList");
                        }

                    }
                    dto.IsActive = true;
                    int typee = (int)dto.AdproTargetType;

                    if (typee == 2)
                    {
                        dto.TargetTopCategoryId = null;
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetBrandId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef markayı seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 3)
                    {
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Hedef olarak orta kategoriyi seçtiniz ancak kategorileri boş bıraktınız. Lütfen hedef ana ve orta kategoriyi seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                        if (_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                        {
                            HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 4)
                    {
                        dto.TargetTopCategoryId = null;
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        if (dto.TargetProductName == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef ürün adını seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 0)
                    {
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null || dto.TargetSubCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Hedef olarak alt kategoriyi seçtiniz ancak kategorileri boş bıraktınız.Lütfen hedef ana, orta ve alt kategorilerini seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                        if (_subRepo.GetSubCateByIdWithJoin((int)dto.TargetSubCategoryId).MiddleCategoryId != dto.TargetMiddleCategoryId || _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                        {
                            HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 1)
                    {
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef ana kategoriyi seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    string page = dto.Culture.Equals("tr") ? "sayfa" : "page";
                    string categor = dto.Culture.Equals("tr") ? "kategoriler" : "categories";
                    string bran = dto.Culture.Equals("tr") ? "markalar" : "brands";
                    string prodeta = dto.Culture.Equals("tr") ? "urun-detay" : "product-detail";

                    if (dto.Culture.Equals("tr"))
                    {
                        dto.PhotoAltTag =
                            typee == 2 ? _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " Markasına ait ürünler premiummedikal.com web sitesinde satılmaktadır"
                            : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleTR
                            : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleTR
                            : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleTR
                            : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleTR;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlTR}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlTR}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlTR}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).Brand.BandNameUrl}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).TopCategory.TopCategoryNameUrlTR}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).MiddleCategory?.MiddleCategoryNameUrlTR}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).SubCategory?.SubCategoryNameUrlTR}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).ProductNameUrlTR}";
                    }
                    else if (dto.Culture.Equals("ru"))
                    {
                        dto.PhotoAltTag =
                           typee == 2 ? "Продукты бренда " + _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " продаются на сайте premiummedikal.com"
                           : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleRU
                           : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleRU
                           : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleRU
                           : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleRU;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).Brand.BandNameUrl}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).ProductNameUrlEN}";
                    }
                    else
                    {
                        dto.PhotoAltTag =
                           typee == 2 ? _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " Brand products are sold on premiummedikal.com website"
                           : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleEN
                           : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleEN
                           : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleEN
                           : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleEN;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).Brand.BandNameUrl}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).ProductNameUrlEN}";
                    }
                    dto.AdproHref = dto.AdproHref.Replace("///", "/", StringComparison.Ordinal);
                    dto.AdproHref = dto.AdproHref.Replace("//", "/", StringComparison.Ordinal);

                    var adproDb = _mapper.Map<AdProduct>(dto);
                    bool a = _adproRepo.CreateEntity(adproDb);

                    bool b = true;

                    if (dto.AdProductPhoto != null)
                    {
                        b = ProcessAdProductPhoto(dto.AdProductPhoto, adproDb.Id);
                    }
                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", " Reklam oluşturma işlemi başarılı.");
                        return RedirectToAction("AdProductList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", " Reklam oluşturulurken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin AdProduct CreateAdProduct satır 239",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("AdProductList");
                    }
                }

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("AdProductList");
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " , " + e.InnerException + " " + messages,
                    ErrorLocation = "AdProductController CreateAdProduct",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", $"Hata oluştu ve kayıt altına alındı.");
                return RedirectToAction("AdProductList");

            }
        }

        public IActionResult UpdateAdProduct(int id)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var adproDb = _adproRepo.GetEntityById(id);
            var adproDto = _mapper.Map<AdminAdProductDto>(adproDb);
            if (adproDto.TargetProductId != null)
            {
                adproDto.TargetProductName = _productRepo.GetEntityById((int)adproDto.TargetProductId).NameTR;
            }
            adproDto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            adproDto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
            return PartialView(adproDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAdProduct(AdminAdProductDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(dto.TargetProductName))
                    {
                        var pro = _productRepo.GetProductWithNameTR(dto.TargetProductName);
                        if (pro == null)
                        {
                            HttpContext.Session.SetString("warning", " Girilen hedef ürün adı bulunamadı. Lütfen hedef ürün adını kontrol ediniz.");
                            return RedirectToAction("AdProductList");
                        }
                        else
                        {
                            dto.TargetProductId = pro.Id;
                        }
                    }
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

                    if (dto.AdProductPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.AdProductPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("AdProductList");
                        }

                    }

                    dto.IsActive = true;
                    int typee = (int)dto.AdproTargetType;

                    if (typee == 2)
                    {
                        dto.TargetTopCategoryId = null;
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetBrandId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef markayı seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 3)
                    {
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef ana ve orta kategoriyi seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                        if (_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                        {
                            HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 4)
                    {
                        dto.TargetTopCategoryId = null;
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        if (dto.TargetProductName == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef ürün adını seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 0)
                    {
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null || dto.TargetSubCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedeflenen ana, orta ve alt kategorilerin hepsini seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                        if (_subRepo.GetSubCateByIdWithJoin((int)dto.TargetSubCategoryId).MiddleCategoryId != dto.TargetMiddleCategoryId || _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                        {
                            HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    if (typee == 1)
                    {
                        dto.TargetMiddleCategoryId = null;
                        dto.TargetSubCategoryId = null;
                        dto.TargetBrandId = null;
                        dto.TargetProductId = null;
                        if (dto.TargetTopCategoryId == null)
                        {
                            HttpContext.Session.SetString("warning", "Lütfen hedef ana kategoriyi seçiniz.");
                            return RedirectToAction("AdProductList");
                        }
                    }
                    string page = dto.Culture.Equals("tr") ? "sayfa" : "page";
                    string categor = dto.Culture.Equals("tr") ? "kategoriler" : "categories";
                    string bran = dto.Culture.Equals("tr") ? "markalar" : "brands";
                    string prodeta = dto.Culture.Equals("tr") ? "urun-detay" : "product-detail";

                    if (dto.Culture.Equals("tr"))
                    {
                        dto.PhotoAltTag =
                            typee == 2 ? _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " Markasına ait ürünler premiummedikal.com web sitesinde satılmaktadır"
                            : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleTR
                            : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleTR
                            : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleTR
                            : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleTR;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlTR}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlTR}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlTR}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlTR}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithId((int)dto.TargetProductId).Brand.BandNameUrl}/{_productRepo.GetProductWithId((int)dto.TargetProductId).TopCategory.TopCategoryNameUrlTR}/{_productRepo.GetProductWithId((int)dto.TargetProductId).MiddleCategory?.MiddleCategoryNameUrlTR}/{_productRepo.GetProductWithId((int)dto.TargetProductId).SubCategory?.SubCategoryNameUrlTR}/{_productRepo.GetEntityById((int)dto.TargetProductId).ProductNameUrlTR}";
                    }
                    else if (dto.Culture.Equals("ru"))
                    {
                        dto.PhotoAltTag =
                           typee == 2 ? "Продукты бренда " + _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " продаются на сайте premiummedikal.com"
                           : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleRU
                           : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleRU
                           : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleRU
                           : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleRU;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithId((int)dto.TargetProductId).Brand.BandNameUrl}/{_productRepo.GetProductWithId((int)dto.TargetProductId).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetEntityById((int)dto.TargetProductId).ProductNameUrlEN}";
                    }
                    else
                    {
                        dto.PhotoAltTag =
                          typee == 2 ? _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " Brand products are sold on premiummedikal.com website"
                          : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleEN
                          : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleEN
                          : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleEN
                          : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleEN;

                        dto.AdproHref =
                             typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                           : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                           : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                           : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                           : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithId((int)dto.TargetProductId).Brand.BandNameUrl}/{_productRepo.GetProductWithId((int)dto.TargetProductId).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetEntityById((int)dto.TargetProductId).ProductNameUrlEN}";

                    }
                    dto.AdproHref = dto.AdproHref.Replace("///", "/", StringComparison.Ordinal);
                    dto.AdproHref = dto.AdproHref.Replace("//", "/", StringComparison.Ordinal);

                    var adproDb = _mapper.Map<AdProduct>(dto);
                    bool a = _adproRepo.UpdateEntity(adproDb);

                    bool b = true;

                    if (dto.AdProductPhoto != null)
                    {
                        b = ProcessAdProductPhoto(dto.AdProductPhoto, adproDb.Id);
                    }

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", " Reklam Güncelleme işlemi başarılı.");
                        return RedirectToAction("AdProductList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", " Reklam Güncellenirken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin AdProduct UpdateAdProduct satır 420",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("AdProductList");
                    }
                }

                string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                               .Select(v => v.ErrorMessage + " " + v.Exception));

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("AdProductList");
            }
            catch (Exception e)
            {
                string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message+ " , " + e.InnerException +
                    " " + messages,
                    ErrorLocation = "AdProductController UpdateAdProduct Post",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", $"Hata oluştu ve kayıt altına alındı.");
                return RedirectToAction("AdProductList");
            }
        }

        public bool ProcessAdProductPhoto(IFormFile file, int adproId)
        {
            try
            {
                //eğer daha önceden AdProduct main photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var adproDb = _adproRepo.GetEntityById(adproId);

                if (!string.IsNullOrEmpty(adproDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "AdProductImages", adproDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, adproDb.PhotoFileName);

                        if (System.IO.File.Exists(fullPathToDelete))
                        {
                            System.IO.File.Delete(fullPathToDelete);
                        }
                }

                var uniqueFileName = GetUniqueAdProductPhotoFileName(file.FileName, adproId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "AdProductImages", "Raw");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "AdProductImages", adproDb.Culture);
                string fullPathModified = Path.Combine(folderPathModified, uniqueFileName);

                if (!Directory.Exists(folderPathRaw))
                {
                    Directory.CreateDirectory(folderPathRaw);
                }

                using (var fileStream = new FileStream(fullPathRaw, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                FileInfo fs = new FileInfo(fullPathRaw);
                var imageModified = new Bitmap(590, 350);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 590, 350);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 590, 350);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 590, 350);
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                adproDb.PhotoFileName = uniqueFileName;
                bool dd = _adproRepo.UpdateEntity(adproDb);

                if (bb  && dd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin AdProduct ProcessAdProductPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }
        }


        public JsonResult DeleteAdProductPhoto(int adproId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var adproDb = _adproRepo.GetEntityById(adproId);

                string fullPathMain = Path.Combine(_environment.WebRootPath, "Images", "AdProductImages", adproDb.Culture, adproDb.PhotoFileName);

                if (System.IO.File.Exists(fullPathMain))
                {
                    System.IO.File.Delete(fullPathMain);
                }

                adproDb.PhotoFileName = null;
                bool a = _adproRepo.UpdateEntity(adproDb);

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli AdProduct DeleteAdProductPhoto",
                        ErrorDetail = "AdProduct PhotoFileName bilgisi güncellenemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Reklam PhotoFileName bilgisi güncellenemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Reklam fotoğrafı silindi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli AdProduct DeleteAdProductPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Reklam Fotoğrafı silinemedi. Hata oluştu. {e}" });
            }
        }


        private string GetUniqueAdProductPhotoFileName(string fileName, int adproId)
        {
            return "adproId-" + adproId  
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        

        public JsonResult HardDeleteAdProduct(int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var adproDb = _adproRepo.GetEntityById(id);

                if (!string.IsNullOrEmpty(adproDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "AdProductImages", adproDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, adproDb.PhotoFileName);

                    if (System.IO.File.Exists(fullPathToDelete))
                    {
                        System.IO.File.Delete(fullPathToDelete);
                    }
                }

                bool a = _adproRepo.HardDeleteEntity(adproDb);

                if (a)
                {
                    return Json(new { success = true, message = "Reklam kalıcı olarak silindi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Reklam silinemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin AdProduct HardDeleteAdProduct try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return Json(new { success = false, message = "Reklam silinemedi. Hata oluştu." });
            }

        }


        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }
    }
}
