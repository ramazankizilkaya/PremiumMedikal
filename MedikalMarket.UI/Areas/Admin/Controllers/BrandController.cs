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
using System.Reflection;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly ISliderRepository _sliderRepo;
        private readonly IAdProductRepository _adproRepo;


        public BrandController(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IProductRepository productRepo, IMapper mapper, IBrandRepository brandRepo, IWebHostEnvironment environment, ISliderRepository sliderRepo, IAdProductRepository adproRepo)
        {
            _topRepo = topRepo;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _environment = environment;
            _sliderRepo = sliderRepo;
            _adproRepo = adproRepo;
        }

        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return View();
        }

        public IActionResult BrandList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");

            var brandDtos = new List<AdminBrandDto>();
            var brandsDb = _brandRepo.GetAllBrandsWithFullJoin().ToList();
            brandsDb.ForEach(x => brandDtos.Add(_mapper.Map<AdminBrandDto>(x)));
            brandDtos.ForEach(x => x.ProductCount = _brandRepo.GetBrandByIdFullJoin(x.Id).Products.Count);
            return View(brandDtos.OrderByDescending(x => x.Id));
        }

        public IActionResult CreateBrand()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return PartialView(new AdminCreateBrandDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBrand(AdminCreateBrandDto dto)
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

                    if (dto.BrandPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.BrandPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("BrandList");
                        }
                    }
                    dto.IsActive = true;
                    dto.BandNameUrl = dto.BrandName.ConvertToFriendlyUrl();
                    dto.PhotoAltTagTR = dto.BrandName + " Marka Logosu";
                    dto.PhotoAltTagEN = dto.BrandName + " Brand Logo";
                    dto.PhotoAltTagRU = dto.BrandName + " Логотип бренда";
                    dto.MasterPageMetaTitleTR = dto.BrandName + " Markasına ait tüm ürünler premiummedikal.com web sitesinde satılmaktadır.";
                    dto.MasterPageMetaTitleEN = "All " + dto.BrandName + " Brand products are sold on premiummedikal.com website.";
                    dto.MasterPageMetaTitleRU = "Продукты бренда " + dto.BrandName + " продаются на сайте premiummedikal.com";
                    dto.MasterPageMetaDescriptionTR = dto.BrandName + " markasına ait tüm medikal ürünleri %50'ye varan indirim oranlarıyla iki yıl üretici garantisi altında güvenle satın alabilirsiniz.";
                    dto.MasterPageMetaDescriptionEN = $"You can safely purchase all medical products of the {dto.BrandName} brand with a discount rate of up to 50% under the manufacturer's warranty for two years.";
                    dto.MasterPageMetaDescriptionRU = $"Вы можете смело приобретать все лекарственные препараты марки { dto.BrandName} со скидкой до 50 % по гарантии производителя на два года.";

                    var brandDb = _mapper.Map<Brand>(dto);
                    bool a = _brandRepo.CreateEntity(brandDb);

                    bool b = true;

                    if (dto.BrandPhoto != null)
                    {
                        b = ProcessBrandPhoto(dto.BrandPhoto, brandDb.Id);
                    }
                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", " Marka oluşturma işlemi başarılı.");
                        return RedirectToAction("BrandList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", " Marka oluşturulurken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin Brand CreateBrand satır 144",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("BrandList");
                    }
                }

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("BrandList");
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "BrandController CreateBrandPost",
                    ErrorUrl = HttpContext.Request.Path
                });
                return RedirectToAction("BrandList");
            }
        }

        public IActionResult UpdateBrand(int id)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var brandDb = _brandRepo.GetEntityById(id);
            var brandDto = _mapper.Map<AdminBrandDto>(brandDb);
            return PartialView(brandDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBrand(AdminBrandDto dto)
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

                    if (dto.BrandPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.BrandPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("BrandList");
                        }

                    }
                    //dto.IsActive = true;
                    dto.BandNameUrl = dto.BrandName.ConvertToFriendlyUrl();
                    var brandDb = _mapper.Map<Brand>(dto);
                    bool a = _brandRepo.UpdateEntity(brandDb);

                    bool b = true;

                    if (dto.BrandPhoto != null)
                    {
                        b = ProcessBrandPhoto(dto.BrandPhoto, brandDb.Id);
                    }

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", " Marka Güncelleme işlemi başarılı.");
                        return RedirectToAction("BrandList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", " Marka Güncellenirken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin Brand UpdateBrand satır 180",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("BrandList");
                    }
                }

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("BrandList");

            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "BrandController UpdateBrandPost",
                    ErrorUrl = HttpContext.Request.Path
                });
                return RedirectToAction("BrandList");
            }
        }

        public bool ProcessBrandPhoto(IFormFile file, int brandId)
        {
            try
            {
                //eğer daha önceden Brand main photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var brandDb = _brandRepo.GetEntityById(brandId);

                if (!string.IsNullOrEmpty(brandDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "BrandImages");
                    string fullPathToDelete = Path.Combine(folderPathToDelete, brandDb.PhotoFileName);

                        if (System.IO.File.Exists(fullPathToDelete))
                        {
                            System.IO.File.Delete(fullPathToDelete);
                        }
                }

                var uniqueFileName = GetUniqueBrandPhotoFileName(file.FileName, brandId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "BrandImages", "Raw");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "BrandImages");
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
                var imageModified = new Bitmap(100, 100);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 100, 100);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 100, 100);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 100, 100);
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                brandDb.PhotoFileName = uniqueFileName;
                bool dd = _brandRepo.UpdateEntity(brandDb);

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
                    ErrorLocation = "Admin Brand ProcessBrandPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }
        }

        public IActionResult ValidateBrandName(string BrandName)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var brandDb = _brandRepo.GetAllEntities().Where(x => x.BrandName.Equals(BrandName)).ToList();
            if (brandDb.Count == 0)
            {
                return Json(true);
            }
            return Json(false);
        }
        public JsonResult DeleteBrandPhoto(int brandId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var brandDb = _brandRepo.GetEntityById(brandId);

                string fullPathMain = Path.Combine(_environment.WebRootPath, "Images", "BrandImages", brandDb.PhotoFileName);

                if (System.IO.File.Exists(fullPathMain))
                {
                    System.IO.File.Delete(fullPathMain);
                }
                
                brandDb.PhotoFileName = null;
                bool a = _brandRepo.UpdateEntity(brandDb);

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli Brand DeleteBrandPhoto",
                        ErrorDetail = "Brand PhotoFileName bilgisi güncellenemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Marka PhotoFileName bilgisi güncellenemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Marka fotoğrafı silindi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli Brand DeleteBrandPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Marka Fotoğrafı silinemedi. Hata oluştu. {e}" });
            }
        }

        public void HardDeleteBrandPhoto(string brandPhotoFileName)
        {
                string fullPathMain = Path.Combine(_environment.WebRootPath, "Images", "BrandImages", brandPhotoFileName);

                if (System.IO.File.Exists(fullPathMain))
                {
                    System.IO.File.Delete(fullPathMain);
                }
        }
        private string GetUniqueBrandPhotoFileName(string fileName, int brandId)
        {
            return "brandId-" + brandId + "_" 
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        public JsonResult DeleteBrand(int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var brandDb = _brandRepo.GetBrandByIdFullJoin(id);
                var sliders = _sliderRepo.FindEntities(x => x.SliderTargetType == Data.Enums.SliderTargetType.BrandMasterPage && x.TargetBrandId == brandDb.Id).ToList();
                var adpros = _adproRepo.FindEntities(x => x.AdproTargetType == Data.Enums.SliderTargetType.BrandMasterPage && x.TargetBrandId == brandDb.Id).ToList();
                
                if (sliders.Count>0)
                {
                    sliders.ForEach(x => HardDeleteSlider(x.Id));
                }
                if (adpros.Count > 0)
                {
                    adpros.ForEach(x => HardDeleteAdProduct(x.Id));
                }

               var products = _productRepo.FindEntities(x => x.BrandId == brandDb.Id).ToList();
                bool a = true;
                bool b = true;
                if (products.Count == 0)
                {
                    HardDeleteBrandPhoto(brandDb.PhotoFileName);
                    a = _brandRepo.HardDeleteEntity(brandDb);
                }
                if (products.Count > 0)
                {
                    a = _productRepo.SoftDeleteGivenEntities(products);
                    b = _brandRepo.SoftDeleteEntity(brandDb);
                }
                
                if (a && b)
                {
                    return Json(new { success = true, message = $"{brandDb.BrandName} markası ve bu markaya ait {brandDb.Products.Count} adet ürün,{sliders.Count} adet slider {adpros.Count} adet reklam  silindi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = $"{brandDb.BrandName} marka silinemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Brand DeleteBrand try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return Json(new { success = false, message = "Marka silinemedi. Hata oluştu." });
            }
        }

        public JsonResult UndeleteBrand(int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var brandDb = _brandRepo.GetBrandByIdFullJoin(id);
                bool a = true;
                brandDb.IsDeleted = false;
                a = _brandRepo.UpdateEntity(brandDb);
                var products = _productRepo.FindEntities(x => x.BrandId == brandDb.Id).ToList();
               
                bool b = true;
                if (products.Count > 0)
                {
                    products.ForEach(x => x.IsDeleted = false);
                    b = _productRepo.UpdateEntities(products);
                }

                if (a && b)
                {
                    return Json(new { success = true, message = $"{brandDb.BrandName} markası ve bu markaya ait {brandDb.Products.Count} adet ürünün silindi etiketi kaldırıldı.", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = $"{brandDb.BrandName} marka silinme işlemi geri alınamadı. Marka fotoğraf adı güncelleme işlemi:{a}, Markaya ait ürünleri güncelleme işlemi:{b}"});
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Brand UndeleteBrand try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return Json(new { success = false, message = "Marka silinme işlemi geri alınamadı. Hata oluştu." });
            }

        }

        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }


        public void HardDeleteSlider(int id)
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

                _sliderRepo.HardDeleteEntity(sliderDb);
                
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
            }

        }

        public void HardDeleteAdProduct(int id)
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

                _adproRepo.HardDeleteEntity(adproDb);
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
            }

        }
    }
}
