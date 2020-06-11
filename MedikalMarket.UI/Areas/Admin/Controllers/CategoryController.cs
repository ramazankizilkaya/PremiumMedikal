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
using System.IO;
using System.Linq;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly ISliderRepository _sliderRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IAdProductRepository _adproRepo;

        public CategoryController(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IProductRepository productRepo, IMapper mapper, ISliderRepository sliderRepo, IWebHostEnvironment environment, IAdProductRepository adproRepo)
        {
            _topRepo = topRepo;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _mapper = mapper;
            _sliderRepo = sliderRepo;
            _adproRepo = adproRepo;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id, string type)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            if (type==null)
            {
                return PartialView();
            }
            if (type.Equals("ana"))
            {
                var cateDb = _topRepo.GetTopCateById(id);
                var dto = new AdminCateDto();
                dto = _mapper.Map<AdminCateDto>(cateDb);
                return PartialView(dto);
            }
            else if (type.Equals("orta"))
            {
                var cateDb = _midRepo.GetEntityById(id);
                var dto = new AdminCateDto();
                dto = _mapper.Map<AdminCateDto>(cateDb);
                return PartialView(dto);
            }
            else if (type.Equals("alt"))
            {
                var cateDb = _subRepo.GetEntityById(id);
                var dto = new AdminCateDto();
                dto = _mapper.Map<AdminCateDto>(cateDb);
                return PartialView(dto);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpGet]
        public IActionResult CreateCategory(int id, string type)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            if (type.Equals("root"))
            {
                return PartialView(new AdminCateDto { Id=0, CategoryType = CategoryType.top, TopCategoryId = 0, MiddleCategoryId = 0 });
            }
            if (type.Equals("ana"))
            {
                return PartialView(new AdminCateDto { Id = 0, CategoryType = CategoryType.mid, TopCategoryId = id, MiddleCategoryId = 0 });

            }
            else if (type.Equals("orta"))
            {
                var topCateId = _midRepo.GetEntityById(id).TopCategoryId;
                return PartialView(new AdminCateDto { Id = 0, CategoryType = CategoryType.sub, TopCategoryId = topCateId, MiddleCategoryId = id });
            }
            
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(AdminCateDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
            try
            {
                dto.Id = 0;
                dto.HeadDescriptionTR = $"{dto.NameTR} kategorisinde %50'ye varan indirimli medikal ürünler, premiummedikal.com";
                dto.HeadDescriptionEN = $"Up to 50% discounted medical products in the {dto.NameEN} category, premiummedikal.com";
                dto.HeadDescriptionRU = $"Медицинские изделия со скидкой до 50% в категории {dto.NameRU}, premiummedikal.com";
                dto.HeadTitleTR = dto.NameTR;
                dto.HeadTitleEN = dto.NameEN;
                dto.HeadTitleRU = dto.NameRU;

                if (dto.CategoryType == CategoryType.top)
                {
                    if (_topRepo.AnyEntity(x => x.NameTR.Equals(dto.NameTR)) || _topRepo.AnyEntity(x => x.NameTR.Equals(dto.NameEN)) || _topRepo.AnyEntity(x => x.NameRU.Equals(dto.NameRU)))
                    {
                        HttpContext.Session.SetString("warning", "Eklenmek istenen ana kategorinin Türkçe, İngilizce veya Rusça adı kullanımdadır. Lütfen ana kategori isimlerinin unique olmasına dikkat ediniz.");
                        return RedirectToAction("CategoryList");
                    }
                    var cateDb = _mapper.Map<TopCategory>(dto);
                    bool a = _topRepo.CreateEntity(cateDb);

                    cateDb.TopCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.TopCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.TopCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _topRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori ekleme işlemi başarılı.");
                        return RedirectToAction("CategoryList");
                    }
                }
                if (dto.CategoryType == CategoryType.mid)
                {
                    if (_midRepo.AnyEntity(x => x.NameTR.Equals(dto.NameTR) && x.TopCategoryId == dto.TopCategoryId) || _midRepo.AnyEntity(x => x.NameEN.Equals(dto.NameEN) && x.TopCategoryId == dto.TopCategoryId) || _midRepo.AnyEntity(x => x.NameRU.Equals(dto.NameRU) && x.TopCategoryId == dto.TopCategoryId))
                    {
                        HttpContext.Session.SetString("warning", "Eklenmek istenen orta kategorinin Türkçe, İngilizce veya Rusça adı kullanımdadır. Lütfen orta kategori isimlerinin unique olmasına dikkat ediniz.");
                        return RedirectToAction("CategoryList");
                    }

                    var cateDb = _mapper.Map<MiddleCategory>(dto);
                    bool a = _midRepo.CreateEntity(cateDb);

                    cateDb.MiddleCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.MiddleCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.MiddleCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _midRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori ekleme işlemi başarılı.");
                        return RedirectToAction("CategoryList");
                    }
                }
                if (dto.CategoryType == CategoryType.sub)
                {
                    if (_subRepo.AnyEntity(x => x.NameTR.Equals(dto.NameTR) && x.MiddleCategoryId == dto.MiddleCategoryId) || _subRepo.AnyEntity(x => x.NameEN.Equals(dto.NameEN) && x.MiddleCategoryId == dto.MiddleCategoryId) || _subRepo.AnyEntity(x => x.NameRU.Equals(dto.NameRU) && x.MiddleCategoryId == dto.MiddleCategoryId))
                    {
                        HttpContext.Session.SetString("warning", "Eklenmek istenen alt kategorinin Türkçe, İngilizce veya Rusça adı kullanımdadır. Lütfen alt kategori isimlerinin unique olmasına dikkat ediniz.");
                        return RedirectToAction("CategoryList");
                    }

                    var cateDb = _mapper.Map<SubCategory>(dto);
                    bool a = _subRepo.CreateEntity(cateDb);

                    cateDb.SubCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.SubCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.SubCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _subRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori ekleme işlemi başarılı.");
                        return RedirectToAction("CategoryList");
                    }
                }

                HttpContext.Session.SetString("warning", "Kategori ekleme işleminde hata oluştu.");
                return RedirectToAction("CategoryList");
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "CategoryController CreateCategory",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", "Kategori ekleme işleminde hata oluştu.");
                return RedirectToAction("CategoryList");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCategory(AdminCateDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));

            try
            {
                dto.HeadDescriptionTR = $"{dto.NameTR} kategorisinde %50'ye varan indirimli medikal ürünler, premiummedikal.com";
                dto.HeadDescriptionEN = $"Up to 50% discounted medical products in the {dto.NameEN} category, premiummedikal.com";
                dto.HeadDescriptionRU = $"Медицинские изделия со скидкой до 50% в категории {dto.NameRU}, premiummedikal.com";
                dto.HeadTitleTR = dto.NameTR;
                dto.HeadTitleEN = dto.NameEN;
                dto.HeadTitleRU = dto.NameRU;

                if (dto.CategoryType == Data.Enums.CategoryType.top)
                {
                    var cateDb = _mapper.Map<TopCategory>(dto);
                    bool a = _topRepo.UpdateEntity(cateDb);

                    cateDb.TopCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.TopCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.TopCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _topRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori bilgileri başarıyla güncellendi.");
                        return RedirectToAction("CategoryList");
                    }
                }
                if (dto.CategoryType == Data.Enums.CategoryType.mid)
                {
                    var cateDb = _mapper.Map<MiddleCategory>(dto);
                    bool a = _midRepo.UpdateEntity(cateDb);

                    cateDb.MiddleCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.MiddleCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.MiddleCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _midRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori bilgileri başarıyla güncellendi.");
                        return RedirectToAction("CategoryList");
                    }
                }
                if (dto.CategoryType == Data.Enums.CategoryType.sub)
                {
                    var cateDb = _mapper.Map<SubCategory>(dto);
                    bool a = _subRepo.UpdateEntity(cateDb);

                    cateDb.SubCategoryNameUrlTR = cateDb.NameTR.ConvertToFriendlyUrl();
                    cateDb.SubCategoryNameUrlEN = cateDb.NameEN.ConvertToFriendlyUrl();
                    cateDb.SubCategoryNameUrlRU = cateDb.NameEN.ConvertToFriendlyUrl();
                    bool b = _subRepo.UpdateEntity(cateDb);

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Kategori bilgileri başarıyla güncellendi.");
                        return RedirectToAction("CategoryList");
                    }
                }

                HttpContext.Session.SetString("warning", "Kategori Güncelleme işleminde hata oluştu.");
                return RedirectToAction("CategoryList");
            }
            catch (Exception e )
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "CategoryController UpdateCategory",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", "Kategori ekleme işleminde hata oluştu.");
                return RedirectToAction("CategoryList");
            }
        }

        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return View();
        }

        public IActionResult CategoryList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");
            var cates = _topRepo.GetAllTopCates();
            return View(cates);
        }

        [HttpGet]
        public JsonResult GetCategoryName(string type, int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                if (type==null)
                {
                    return Json(new { success = false, message = "Lütfen kategori seçiniz." });
                }
                if (type.Equals("root"))
                {
                    return Json(new { success = false, message = "Kök dizin silinemez" });
                }
                if (type.Equals("ana"))
                {
                    var topCate = _topRepo.GetEntityById(id);
                    int productCount = _productRepo.CountEntity(x => x.TopCategoryId == id);
                    int midCateCount = _midRepo.CountEntity(x => x.TopCategoryId == topCate.Id);
                    int subCateCount = _subRepo.GetSubCatesWithJoin().Count(x => x.MiddleCategory.TopCategoryId == topCate.Id);
                    int sliderCount = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.TopCategory && x.TargetTopCategoryId == topCate.Id).Count();
                    int adProCount = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.TopCategory && x.TargetTopCategoryId == topCate.Id).Count();
                    return Json(new { success = true, message = $"Dikkat!!! \"{topCate.NameTR}\" Ana Kategorisi silinecek. İlave olarak bu kategoride bulunan toplam {productCount} adet ürün, {midCateCount} adet orta kategori,  {subCateCount} adet alt kategori, {sliderCount} adet slider ve  {adProCount} adet reklam silinecek. Onaylıyor musunuz?", type="ana", itemId = topCate.Id});
                }

                if (type.Equals("orta"))
                {
                    var midCate = _midRepo.GetEntityById(id);
                    int productCount = _productRepo.CountEntity(x => x.MiddleCategoryId == id);
                    int subCateCount = _subRepo.GetSubCatesWithJoin().Count(x => x.MiddleCategoryId == midCate.Id);
                    int sliderCount = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.MiddleCategory && x.TargetMiddleCategoryId == midCate.Id).Count();
                    int adProCount = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.MiddleCategory && x.TargetMiddleCategoryId == midCate.Id).Count();

                    return Json(new { success = true, message = $"Dikkat!!! \"{midCate.NameTR}\" Orta Kategorisi silinecek. İlave olarak bu kategoride bulunan toplam {productCount} adet ürün, {subCateCount} adet alt kategori, {sliderCount} adet slider ve {adProCount} adet reklam silinecek. Onaylıyor musunuz?", type = "orta", itemId = midCate.Id });
                }

                if (type.Equals("alt"))
                {
                    var subCate = _subRepo.GetEntityById(id);
                    int productCount = _productRepo.CountEntity(x => x.SubCategoryId == id);
                    int sliderCount = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.SubCategory && x.TargetSubCategoryId == subCate.Id).Count();
                    int adProCount = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.SubCategory && x.TargetSubCategoryId == subCate.Id).Count();
                    return Json(new { success = true, message = $"Dikkat!!! \"{subCate.NameTR}\" Alt Kategorisi silinecek. İlave olarak bu kategoride bulunan toplam {productCount} adet ürün, {sliderCount} adet slider ve {adProCount} adet reklam silinecek. Onaylıyor musunuz?", type = "alt", itemId = subCate.Id });
                }
                else
                {
                    return Json(new { success = false, message = "Kategori bulunamadı" });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation= "Admin paneli CategoryController GetCategoryName",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                return Json(new { success = false, message = $"Hata oluştu. Hata kayıt altına alındı. {e}" });
            }

        }

        [HttpPost]
        public JsonResult DeleteCategory(string categoryType, int categoryId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                if (categoryType.Equals("root"))
                {
                    return Json(new { success = false, message = "kök dizin silinemez" });
                }
                if (categoryType.Equals("ana"))
                {
                    var topCate = _topRepo.GetEntityById(categoryId);
                    var midCates = _midRepo.FindEntities(x => x.TopCategoryId == topCate.Id).ToList();
                    var subCates = _subRepo.GetSubCatesWithJoin().Where(x => x.MiddleCategory.TopCategoryId == topCate.Id).ToList();

                    var products = _productRepo.FindEntities(x => x.TopCategoryId == categoryId).ToList();
                    var sliders = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.TopCategory && x.TargetTopCategoryId == topCate.Id).ToList();
                    var adPros = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.TopCategory && x.TargetTopCategoryId == topCate.Id).ToList();

                    bool e = true;
                    if (adPros.Count > 0)
                    {
                        adPros.ForEach(x => e = HardDeleteAdProduct(x.Id));
                    }

                    bool b = true;
                    if (sliders.Count > 0)
                    {
                        sliders.ForEach(x => b = HardDeleteSlider(x.Id));
                    }

                    if (products.Count==0)
                    {
                        _subRepo.HardDeleteGivenEntities(subCates);
                        _midRepo.HardDeleteGivenEntities(midCates);
                        _topRepo.HardDeleteEntity(topCate);
                        return Json(new { success = true });
                    }
                    bool a = true;
                    if (products.Count>0)
                    {
                        products.ForEach(x => x.IsDeleted = true);
                        a = _productRepo.UpdateEntities(products);
                    }

                    bool d = true;
                    if (subCates.Count > 0)
                    {
                        foreach (var sub in subCates)
                        {
                            if (sub.Products.Count == 0)
                            {
                                d = _subRepo.HardDeleteEntity(sub);
                            }
                            else
                            {
                                subCates.ForEach(x => x.IsDeleted = true);
                                d = _subRepo.UpdateEntities(subCates);
                            }
                        }
                    }

                    bool c = true;
                    if (midCates.Count>0)
                    {
                        foreach (var mid in midCates)
                        {
                            if (mid.Products.Count == 0)
                            {
                                c = _midRepo.HardDeleteEntity(mid);
                            }
                            else
                            {
                                midCates.ForEach(x => x.IsDeleted = true);
                                d = _midRepo.UpdateEntities(midCates);
                            }
                        }
                    }
                    
                    topCate.IsDeleted = true;
                    bool f=_topRepo.UpdateEntity(topCate);
                    if (a && b && c && d && e && f)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            ErrorLocation = "Admin paneli CategoryController DeleteCategory",
                            ErrorDetail = $"Ana kategori {topCate.NameTR} silme hatası a:{a}, b:{b}, c:{c}, d:{d}, e:{e}, f:{f}",
                            ErrorUrl = HttpContext.Request.Path
                        });
                        return Json(new { success = false });
                    }
                }

                if (categoryType.Equals("orta"))
                {
                    var midCate = _midRepo.GetEntityById(categoryId);
                    var subCates = _subRepo.GetSubCatesWithJoin().Where(x => x.MiddleCategoryId == categoryId).ToList();
                    var products = _productRepo.FindEntities(x => x.MiddleCategoryId == categoryId).ToList();
                    var sliders = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.MiddleCategory && x.TargetMiddleCategoryId == midCate.Id).ToList();
                    var adPros = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.MiddleCategory && x.TargetMiddleCategoryId == midCate.Id).ToList();
                    
                    bool d = true;
                    if (adPros.Count > 0)
                    {
                        adPros.ForEach(x => d = HardDeleteAdProduct(x.Id));
                    }
                    bool b = true;
                    if (sliders.Count > 0)
                    {
                        sliders.ForEach(x => b = HardDeleteSlider(x.Id));
                    }

                    if (products.Count == 0)
                    {
                        _subRepo.HardDeleteGivenEntities(subCates);
                        _midRepo.HardDeleteEntity(midCate);
                        return Json(new { success = true });
                    }

                    bool a = true;
                    if (products.Count > 0)
                    {
                        products.ForEach(x => x.IsDeleted = true);
                        a = _productRepo.UpdateEntities(products);
                    }

                    bool c = true;
                    if (subCates.Count > 0)
                    {
                        foreach (var sub in subCates)
                        {
                            if (sub.Products.Count == 0)
                            {
                                c = _subRepo.HardDeleteEntity(sub);
                            }
                            else
                            {
                                subCates.ForEach(x => x.IsDeleted = true);
                                c = _subRepo.UpdateEntities(subCates);
                            }
                        }
                    }
                    
                    midCate.IsDeleted = true;
                    bool e= _midRepo.UpdateEntity(midCate);

                    if (a && b && c && d && e)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            ErrorLocation = "Admin paneli CategoryController DeleteCategory",
                            ErrorDetail = $"Orta kategori {midCate.NameTR} silme hatası a:{a}, b:{b}, c:{c}, d:{d}, e:{e}",
                            ErrorUrl = HttpContext.Request.Path
                        });
                        return Json(new { success = false });
                    }
                }

                if (categoryType.Equals("alt"))
                {
                    var subCate = _subRepo.GetEntityById(categoryId);
                    var products = _productRepo.FindEntities(x => x.SubCategoryId == categoryId).ToList();
                    var sliders = _sliderRepo.FindEntities(x => x.SliderTargetType == SliderTargetType.SubCategory && x.TargetSubCategoryId == subCate.Id).ToList();
                    var adPros = _adproRepo.FindEntities(x => x.AdproTargetType == SliderTargetType.SubCategory && x.TargetSubCategoryId == subCate.Id).ToList();

                    bool b = true;
                    if (sliders.Count > 0)
                    {
                        sliders.ForEach(x => b = HardDeleteSlider(x.Id));
                    }
                    bool c = true;
                    if (adPros.Count > 0)
                    {
                        adPros.ForEach(x => c = HardDeleteAdProduct(x.Id));
                    }

                    if (products.Count == 0)
                    {
                        _subRepo.HardDeleteEntity(subCate);
                        return Json(new { success = true });
                    }

                    bool a = true;
                    if (products.Count > 0)
                    {
                        products.ForEach(x => x.IsDeleted = true);
                        a = _productRepo.UpdateEntities(products);
                    }
                    
                    subCate.IsDeleted = true;
                    bool d= _subRepo.UpdateEntity(subCate);

                    if (a && b && c && d)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            ErrorLocation = "Admin paneli CategoryController DeleteCategory",
                            ErrorDetail = $"Alt kategori {subCate.NameTR} silme hatası a:{a}, b:{b}, c:{c}, d:{d}",
                            ErrorUrl = HttpContext.Request.Path
                        });
                        return Json(new { success = false });
                    }
                }

                else
                {
                    return Json(new { success = false, message = "Kategori bulunamadı" });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "Admin paneli CategoryController DeleteCategory",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                return Json(new { success = false, message = $"Hata oluştu. Hata kayıt altına alındı. {e}" });
            }
        }

        public bool HardDeleteAdProduct(int id)
        {
            try
            {
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

                return _adproRepo.HardDeleteEntity(adproDb);

            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin AdProduct CategoryController HardDeleteAdProduct try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false ;
            }

        }

        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }


        public bool HardDeleteSlider(int id)
        {
            try
            {
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

                return  _sliderRepo.HardDeleteEntity(sliderDb);
                
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

                return false;
            }

        }
    }
}
