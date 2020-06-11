using AutoMapper;
using MedikalMarket.UI.Areas.Admin.Dtos;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly ISliderRepository _sliderRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public SliderController(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IProductRepository productRepo, IMapper mapper, ISliderRepository sliderRepo, IBrandRepository brandRepo, IWebHostEnvironment environment)
        {
            _topRepo = topRepo;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _mapper = mapper;
            _sliderRepo = sliderRepo;
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

        public IActionResult SliderList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");

            var sliderDtos = new List<AdminSliderDto>();
            var slidersDb = _sliderRepo.FindEntitiesNoTrack(x=>!x.IsDeleted).ToList();
            slidersDb.ForEach(x => sliderDtos.Add(_mapper.Map<AdminSliderDto>(x)));
            sliderDtos.ForEach(x => x.GenericTargetName = ReturnGenericName(x.Id));
            return View(sliderDtos.OrderByDescending(x => x.Id));
        }

        public string ReturnGenericName(int sliderId)
        {
            var sliderDb = _sliderRepo.GetEntityById(sliderId);
            //Ana
            if (sliderDb.TargetTopCategoryId != null && sliderDb.TargetMiddleCategoryId == null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetBrandId == null && sliderDb.TargetProductId == null)
            {
                return _topRepo.GetEntityById((int)sliderDb.TargetTopCategoryId).NameTR;
            }
            //Orta
            else if(sliderDb.TargetMiddleCategoryId != null && sliderDb.TargetTopCategoryId != null && sliderDb.TargetSubCategoryId == null && sliderDb.TargetBrandId == null && sliderDb.TargetProductId == null)
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

        public IActionResult UpdateSlider(int id)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var sliderDb = _sliderRepo.GetEntityById(id);
            var sliderDto = _mapper.Map<AdminSliderDto>(sliderDb);
            if (sliderDto.TargetProductId!=null)
            {
                sliderDto.TargetProductName = _productRepo.GetEntityById((int)sliderDto.TargetProductId).NameTR;
            }
            sliderDto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
            return PartialView(sliderDto);
        }

        public IActionResult CreateSlider()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var sliderDto = new AdminCreateSliderDto();
            sliderDto.PhotoAltTag = "tag";
            sliderDto.TopCategories = _topRepo.FindEntities(x=>!x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            sliderDto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
            return PartialView(sliderDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSlider(AdminCreateSliderDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            if (ModelState.IsValid)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

                if (dto.SliderPhoto != null)
                {
                    FileInfo fs = new FileInfo(dto.SliderPhoto.FileName);
                    string ext = Path.GetExtension(fs.Extension);

                    if (!allowedExtensions.Contains(ext.ToLower()))
                    {
                        HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        return RedirectToAction("SliderList");
                    }

                }
                if (dto.SliderThumbPhoto != null)
                {
                    FileInfo fs = new FileInfo(dto.SliderThumbPhoto.FileName);
                    string ext = Path.GetExtension(fs.Extension);

                    if (!allowedExtensions.Contains(ext.ToLower()))
                    {
                        HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        return RedirectToAction("SliderList");
                    }
                }
                dto.IsActive = true;
                int typee = (int)dto.SliderTargetType;

                //Hedef Marka ise
                if (typee == 2)
                {
                    dto.TargetTopCategoryId = null;
                    dto.TargetMiddleCategoryId = null;
                    dto.TargetSubCategoryId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetBrandId == null)
                    {
                        HttpContext.Session.SetString("warning", "Lütfen hedef markayı seçiniz.");
                        return RedirectToAction("SliderList");
                    }
                }
                //Hedef orta kategori ise
                if (typee == 3)
                {
                    dto.TargetSubCategoryId = null;
                    dto.TargetBrandId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null)
                    {
                        HttpContext.Session.SetString("warning", "Hedef olarak orta kategoriyi seçtiniz ancak kategorileri boş bıraktınız. Lütfen hedef ana ve orta kategoriyi seçiniz.");
                        return RedirectToAction("SliderList");
                    }
                    if (_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                    {
                        HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                        return RedirectToAction("SliderList");
                    }
                }
                //Hedef ürün ise
                if (typee == 4)
                {
                    dto.TargetTopCategoryId = null;
                    dto.TargetMiddleCategoryId = null;
                    dto.TargetSubCategoryId = null;
                    dto.TargetBrandId = null;
                    if (dto.TargetProductName == null)
                    {
                        HttpContext.Session.SetString("warning", "Lütfen hedef ürün adını seçiniz.");
                        return RedirectToAction("SliderList");
                    }
                }
                //hedef alt kategori ise
                if (typee == 0)
                {
                    dto.TargetBrandId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null || dto.TargetSubCategoryId == null)
                    {
                        HttpContext.Session.SetString("warning", "Hedef olarak alt kategoriyi seçtiniz ancak kategorileri boş bıraktınız.Lütfen hedef ana, orta ve alt kategorilerini seçiniz.");
                        return RedirectToAction("SliderList");
                    }
                    if (_subRepo.GetSubCateByIdWithJoin((int)dto.TargetSubCategoryId).MiddleCategoryId != dto.TargetMiddleCategoryId || _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                    {
                        HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                        return RedirectToAction("SliderList");
                    }
                }

                //hedef ana kategori ise
                if (typee == 1)
                {
                    dto.TargetMiddleCategoryId = null;
                    dto.TargetSubCategoryId = null;
                    dto.TargetBrandId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetTopCategoryId == null)
                    {
                        HttpContext.Session.SetString("warning", "Lütfen hedef ana kategoriyi seçiniz.");
                        return RedirectToAction("SliderList");
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

                    dto.SliderHref =
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

                    dto.SliderHref =
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

                    dto.SliderHref =
                         typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                       : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                       : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                       : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                       : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).Brand.BandNameUrl}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetProductWithNameTR(dto.TargetProductName).ProductNameUrlEN}";
                }
                dto.SliderHref = dto.SliderHref.Replace("///", "/", StringComparison.Ordinal);
                dto.SliderHref = dto.SliderHref.Replace("//", "/", StringComparison.Ordinal);

                var slidersDb = _mapper.Map<Slider>(dto);
                bool a = _sliderRepo.CreateEntity(slidersDb);

                bool b = true;
                bool c = true;

                if (dto.SliderPhoto != null)
                {
                    b = ProcessSliderMainPhoto(dto.SliderPhoto, slidersDb.Id);
                }
                if (dto.SliderThumbPhoto != null)
                {
                    c = ProcessSliderThumbPhoto(dto.SliderThumbPhoto, slidersDb.Id);
                }
                if (a && b && c)
                {
                    HttpContext.Session.SetString("success", " Slider oluşturma işlemi başarılı.");
                    return RedirectToAction("SliderList");
                }
                else
                {
                    HttpContext.Session.SetString("warning", " Slider oluşturulurken hata oluştu. Hata kayıt altına alındı.");

                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Slider CreateSlider satır 214",
                        ErrorDetail = $"a:{a}, b:{b}, c:{c}",
                        ErrorUrl = HttpContext.Request.Path,
                    });
                    return RedirectToAction("SliderList");
                }
            }

            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                           .Select(v => v.ErrorMessage + " " + v.Exception));

            HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
            return RedirectToAction("SliderList");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSlider(AdminSliderDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(dto.TargetProductName))
                {
                    var pro = _productRepo.GetProductWithNameTR(dto.TargetProductName);
                    if (pro==null)
                    {
                        HttpContext.Session.SetString("warning", " Girilen hedef ürün adı bulunamadı. Lütfen hedef ürün adını kontrol ediniz.");
                        return RedirectToAction("SliderList");
                    }
                    else
                    {
                        dto.TargetProductId = pro.Id;
                    }
                }
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

                if (dto.SliderPhoto != null)
                {
                    FileInfo fs = new FileInfo(dto.SliderPhoto.FileName);
                    string ext = Path.GetExtension(fs.Extension);

                    if (!allowedExtensions.Contains(ext.ToLower()))
                    {
                        HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        return RedirectToAction("SliderList");
                    }

                }
                if (dto.SliderThumbPhoto != null)
                {
                    FileInfo fs = new FileInfo(dto.SliderThumbPhoto.FileName);
                    string ext = Path.GetExtension(fs.Extension);

                    if (!allowedExtensions.Contains(ext.ToLower()))
                    {
                        HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        return RedirectToAction("SliderList");
                    }
                }

                dto.IsActive = true;
                int typee = (int)dto.SliderTargetType;

                if (typee == 2)
                {
                    dto.TargetTopCategoryId = null;
                    dto.TargetMiddleCategoryId = null;
                    dto.TargetSubCategoryId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetBrandId==null)
                    {
                        HttpContext.Session.SetString("warning", "Lütfen hedef markayı seçiniz.");
                        return RedirectToAction("SliderList");
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
                        return RedirectToAction("SliderList");
                    }
                    if (_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                    {
                        HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                        return RedirectToAction("SliderList");
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
                        return RedirectToAction("SliderList");
                    }
                }
                if (typee == 0)
                {
                    dto.TargetBrandId = null;
                    dto.TargetProductId = null;
                    if (dto.TargetTopCategoryId == null || dto.TargetMiddleCategoryId == null || dto.TargetSubCategoryId == null)
                    {
                        HttpContext.Session.SetString("warning", "Lütfen hedef ana ve orta ve alt kategorilerini seçiniz.");
                        return RedirectToAction("SliderList");
                    }
                    if (_subRepo.GetSubCateByIdWithJoin((int)dto.TargetSubCategoryId).MiddleCategoryId != dto.TargetMiddleCategoryId || _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).TopCategoryId != dto.TargetTopCategoryId)
                    {
                        HttpContext.Session.SetString("warning", "Seçtiğiniz kategorilerin birbiriyle ilişkili olması gerekmektedir.");
                        return RedirectToAction("SliderList");
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
                        return RedirectToAction("SliderList");
                    }
                }
                string page = dto.Culture.Equals("tr") ? "sayfa" : "page";
                string categor= dto.Culture.Equals("tr") ? "kategoriler" : "categories";
                string bran= dto.Culture.Equals("tr") ? "markalar" : "brands";
                string prodeta= dto.Culture.Equals("tr") ? "urun-detay" : "product-detail";

                if (dto.Culture.Equals("tr"))
                {
                    dto.PhotoAltTag =
                        typee == 2 ? _brandRepo.GetEntityById((int)dto.TargetBrandId).BrandName + " Markasına ait ürünler premiummedikal.com web sitesinde satılmaktadır"
                        : typee == 1 ? _topRepo.GetEntityById((int)dto.TargetTopCategoryId).HeadTitleTR
                        : typee == 3 ? _midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).HeadTitleTR
                        : typee == 0 ? _subRepo.GetEntityById((int)dto.TargetSubCategoryId).HeadTitleTR
                        : _productRepo.GetEntityById((int)dto.TargetProductId).HeadTitleTR;

                    dto.SliderHref =
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

                    dto.SliderHref =
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

                    dto.SliderHref =
                         typee == 2 ? $"/{dto.Culture}/{bran}/{_brandRepo.GetEntityById((int)dto.TargetBrandId).BandNameUrl}/{page}/1"

                       : typee == 1 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{page}/1"

                       : typee == 3 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{page}/1"

                       : typee == 0 ? $"/{dto.Culture}/{categor}/{_topRepo.GetEntityById((int)dto.TargetTopCategoryId).TopCategoryNameUrlEN}/{_midRepo.GetEntityById((int)dto.TargetMiddleCategoryId).MiddleCategoryNameUrlEN}/{_subRepo.GetEntityById((int)dto.TargetSubCategoryId).SubCategoryNameUrlEN}/{page}/1"

                       : $"/{dto.Culture}/{prodeta}/{_productRepo.GetProductWithId((int)dto.TargetProductId).Brand.BandNameUrl}/{_productRepo.GetProductWithId((int)dto.TargetProductId).TopCategory.TopCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).MiddleCategory?.MiddleCategoryNameUrlEN}/{_productRepo.GetProductWithId((int)dto.TargetProductId).SubCategory?.SubCategoryNameUrlEN}/{_productRepo.GetEntityById((int)dto.TargetProductId).ProductNameUrlEN}";

                }
                dto.SliderHref = dto.SliderHref.Replace("///", "/", StringComparison.Ordinal);
                dto.SliderHref= dto.SliderHref.Replace("//", "/", StringComparison.Ordinal);

                var slidersDb = _mapper.Map<Slider>(dto);
                bool a= _sliderRepo.UpdateEntity(slidersDb);

                bool b = true;
                bool c = true;

                if (dto.SliderPhoto!=null)
                {
                    b = ProcessSliderMainPhoto(dto.SliderPhoto, slidersDb.Id);
                }
                if (dto.SliderThumbPhoto != null)
                {
                    c = ProcessSliderThumbPhoto(dto.SliderThumbPhoto, slidersDb.Id);
                }
                if (a && b && c)
                {
                    HttpContext.Session.SetString("success", " Slider Güncelleme işlemi başarılı.");
                    return RedirectToAction("SliderList");
                }
                else
                {
                    HttpContext.Session.SetString("warning", " Slider Güncellenirken hata oluştu. Hata kayıt altına alındı.");

                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Slider UpdateSlider satır 167",
                        ErrorDetail = $"a:{a}, b:{b}, c:{c}",
                        ErrorUrl = HttpContext.Request.Path,
                    });
                    return RedirectToAction("SliderList");
                }
            }

            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                           .Select(v => v.ErrorMessage + " " + v.Exception));

            HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
            return RedirectToAction("SliderList");
        }


        public bool ProcessSliderMainPhoto(IFormFile file, int sliderId)
        {
            try
            {
                //eğer daha önceden slider main photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var sliderDb = _sliderRepo.GetEntityById(sliderId);

                if (!string.IsNullOrEmpty(sliderDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, sliderDb.PhotoFileName);

                        if (System.IO.File.Exists(fullPathToDelete))
                        {
                            System.IO.File.Delete(fullPathToDelete);
                        }
                }

                var uniqueFileName = GetUniqueSliderMainPhotoFileName(file.FileName, sliderId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", "Raw");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
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
                var imageModified = new Bitmap(895, 300);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 895, 300);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 895, 300);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 895, 300);
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                sliderDb.PhotoFileName = uniqueFileName;
                bool dd = _sliderRepo.UpdateEntity(sliderDb);

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
                    ErrorLocation = "Admin Slider ProcessSliderMainPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }
        }

        public bool ProcessSliderThumbPhoto(IFormFile file, int sliderId)
        {
            try
            {
                //eğer daha önceden slider thumb photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var sliderDb = _sliderRepo.GetEntityById(sliderId);

                if (!string.IsNullOrEmpty(sliderDb.ThumbFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, sliderDb.ThumbFileName);

                    if (System.IO.File.Exists(fullPathToDelete))
                    {
                        System.IO.File.Delete(fullPathToDelete);
                    }
                }

                var uniqueFileName = GetUniqueSliderThumbPhotoFileName(file.FileName, sliderId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", "Raw");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
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
                var imageModified = new Bitmap(75, 50);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 75, 50);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 75, 50);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 75, 50);
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                sliderDb.ThumbFileName = uniqueFileName;
                bool dd = _sliderRepo.UpdateEntity(sliderDb);

                if (bb && dd)
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
                    ErrorLocation = "Admin Slider ProcessSliderMainPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }
        }


        public JsonResult DeleteSliderMainPhoto(int sliderId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var sliderDb = _sliderRepo.GetEntityById(sliderId);

                string fullPathMain = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture, sliderDb.PhotoFileName);

                if (System.IO.File.Exists(fullPathMain))
                {
                    System.IO.File.Delete(fullPathMain);
                }

                sliderDb.PhotoFileName = null;
                bool a = _sliderRepo.UpdateEntity(sliderDb);

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli Slider DeleteSliderMainPhoto",
                        ErrorDetail = "Slider PhotoFileName bilgisi güncellenemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Slider PhotoFileName bilgisi güncellenemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Slider fotoğrafı silindi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli Slider DeleteSliderMainPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Slider Fotoğrafı silinemedi. Hata oluştu. {e}" });
            }
        }

        public JsonResult DeleteSliderThumbPhoto(int sliderId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var sliderDb = _sliderRepo.GetEntityById(sliderId);

                string fullPathThumb = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture, sliderDb.ThumbFileName);

                if (System.IO.File.Exists(fullPathThumb))
                {
                    System.IO.File.Delete(fullPathThumb);
                }

                sliderDb.ThumbFileName = null;
                bool a = _sliderRepo.UpdateEntity(sliderDb);

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli Slider DeleteSliderThumbPhoto",
                        ErrorDetail = "Slider ThumbFileName bilgisi güncellenemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Slider ThumbFileName bilgisi güncellenemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Slider thumb fotoğrafı silindi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli Slider DeleteSliderThumbPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Slider Thumb Fotoğrafı silinemedi. Hata oluştu. {e}" });
            }
        }

        public JsonResult SearchProductName(string term)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(term) && !string.IsNullOrEmpty(term))
                {
                    var proNames = _productRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR);
                    return Json(proNames.Take(10));
                }
                else
                {
                    return Json("");
                }
                
            }
            catch
            {
                return Json("");
            }
        }

        private string GetUniqueSliderMainPhotoFileName(string fileName, int sliderId)
        {
            return "SliderId-" + sliderId  
                      + "_Main_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        private string GetUniqueSliderThumbPhotoFileName(string fileName, int sliderId)
        {
            return "SliderId-" + sliderId
                      + "_Thumb_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        public JsonResult HardDeleteSlider(int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var sliderDb = _sliderRepo.GetEntityById(id);

                if (!string.IsNullOrEmpty(sliderDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, sliderDb.PhotoFileName);

                    if (System.IO.File.Exists(fullPathToDelete))
                    {
                        System.IO.File.Delete(fullPathToDelete);
                    }
                }

                if (!string.IsNullOrEmpty(sliderDb.ThumbFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "SliderImages", sliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, sliderDb.ThumbFileName);

                    if (System.IO.File.Exists(fullPathToDelete))
                    {
                        System.IO.File.Delete(fullPathToDelete);
                    }
                }

                bool a = _sliderRepo.HardDeleteEntity(sliderDb);

                if (a)
                {
                    return Json(new { success = true, message = "Slider kalıcı olarak silindi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Slider silinemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Slider HardDeleteSlider try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return Json(new { success = false, message = "Slider silinemedi. Hata oluştu." });
            }

        }



        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }

    }
}
