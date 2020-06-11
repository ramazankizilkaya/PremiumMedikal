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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MiniSliderController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly ISliderRepository _sliderRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IMiniSliderRepository _miniSliderRepo;
        private readonly IWebHostEnvironment _environment;


        public MiniSliderController(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IProductRepository productRepo, IMapper mapper, ISliderRepository sliderRepo, IBrandRepository brandRepo, IWebHostEnvironment environment, IMiniSliderRepository miniSliderRepo)
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
            _miniSliderRepo = miniSliderRepo;
        }
        
        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return View();
        }

        public IActionResult MiniSliderList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");

            var sliderDtos = new List<AdminMiniSliderDto>();
            var slidersDb = _miniSliderRepo.FindEntitiesNoTrack(x=>!x.IsDeleted).ToList();
            slidersDb.ForEach(x => sliderDtos.Add(_mapper.Map<AdminMiniSliderDto>(x)));
            return View(sliderDtos.OrderByDescending(x=>x.Id));
        }

        public IActionResult UpdateMiniSlider(int id)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var sliderDb = _miniSliderRepo.GetEntityById(id);
            var sliderDto = _mapper.Map<AdminMiniSliderDto>(sliderDb);
            return PartialView(sliderDto);
        }

        public IActionResult CreateMiniSlider()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var sliderDto = new AdminMiniSliderDto();
            sliderDto.PhotoAltTag = "tag";
            return PartialView(sliderDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMiniSlider(AdminMiniSliderDto dto)
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

                    if (dto.SliderPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.SliderPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("MiniSliderList");
                        }

                    }

                    var targetProduct = _productRepo.GetProductWithNameTR(dto.TargetProductName);
                    if (targetProduct == null)
                    {
                        HttpContext.Session.SetString("warnng", "Hedef ürün bulunamadı. Lütfen hedef ürün adını doğru giriniz.");
                        return RedirectToAction("MiniSliderList");
                    }
                    dto.IsActive = true;
                    dto.TargetProductId = targetProduct.Id;
                    string page = dto.Culture.Equals("tr") ? "sayfa" : "page";
                    string categor = dto.Culture.Equals("tr") ? "kategoriler" : "categories";
                    string bran = dto.Culture.Equals("tr") ? "markalar" : "brands";
                    string prodeta = dto.Culture.Equals("tr") ? "urun-detay" : "product-detail";

                    if (dto.Culture.Equals("tr"))
                    {
                        dto.SliderHref =
                              $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlTR}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlTR}/{targetProduct.SubCategory?.SubCategoryNameUrlTR}/{targetProduct.ProductNameUrlTR}";
                        dto.PhotoAltTag = targetProduct.PhotoAltTagTR;
                    }
                    else if (dto.Culture.Equals("ru"))
                    {
                        dto.SliderHref =
                            $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlEN}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlEN}/{targetProduct.SubCategory?.SubCategoryNameUrlEN}/{targetProduct.ProductNameUrlEN}";
                        dto.PhotoAltTag = targetProduct.PhotoAltTagRU;
                    }
                    else
                    {
                        dto.SliderHref =
                            $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlEN}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlEN}/{targetProduct.SubCategory?.SubCategoryNameUrlEN}/{targetProduct.ProductNameUrlEN}";
                        dto.PhotoAltTag = targetProduct.PhotoAltTagEN;
                    }
                    dto.SliderHref = dto.SliderHref.Replace("///", "/", StringComparison.Ordinal);
                    dto.SliderHref = dto.SliderHref.Replace("//", "/", StringComparison.Ordinal);

                    var miniSliderDb = _mapper.Map<MiniSlider>(dto);
                    bool a = _miniSliderRepo.CreateEntity(miniSliderDb);

                    bool b = true;

                    if (dto.SliderPhoto != null)
                    {
                        b = ProcessMiniSliderPhoto(dto.SliderPhoto, miniSliderDb.Id);
                    }

                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Mini Slider oluşturma işlemi başarılı.");
                        return RedirectToAction("MiniSliderList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", "Mini Slider oluşturulurken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin MiniSlider CreateMiniSlider satır 152",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("MiniSliderList");
                    }
                }

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("MiniSliderList");
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "MiniSliderController CreateMiniSlider",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", "MiniSlider ekleme işleminde hata oluştu.");
                return RedirectToAction("MiniSliderList");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMiniSlider(AdminMiniSliderDto dto)
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

                    if (dto.SliderPhoto != null)
                    {
                        FileInfo fs = new FileInfo(dto.SliderPhoto.FileName);
                        string ext = Path.GetExtension(fs.Extension);

                        if (!allowedExtensions.Contains(ext.ToLower()))
                        {
                            HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                            return RedirectToAction("MiniSliderList");
                        }

                    }

                    var targetProduct = _productRepo.GetProductWithNameTR(dto.TargetProductName);
                    if (targetProduct == null)
                    {
                        HttpContext.Session.SetString("warning", "Hedef ürün bulunamadı. Lütfen hedef ürün adını doğru giriniz.");
                        return RedirectToAction("MiniSliderList");
                    }

                    dto.IsActive = true;
                    dto.TargetProductId = targetProduct.Id;

                    string page = dto.Culture.Equals("tr") ? "sayfa" : "page";
                    string categor = dto.Culture.Equals("tr") ? "kategoriler" : "categories";
                    string bran = dto.Culture.Equals("tr") ? "markalar" : "brands";
                    string prodeta = dto.Culture.Equals("tr") ? "urun-detay" : "product-detail";

                    if (dto.Culture.Equals("tr"))
                    {
                        dto.PhotoAltTag = targetProduct.PhotoAltTagTR;
                        dto.SliderHref =
                             $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlTR}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlTR}/{targetProduct.SubCategory?.SubCategoryNameUrlTR}/{targetProduct.ProductNameUrlTR}";
                    }
                    else if (dto.Culture.Equals("ru"))
                    {
                        dto.PhotoAltTag = targetProduct.PhotoAltTagRU;
                        dto.SliderHref =
                             $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlEN}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlEN}/{targetProduct.SubCategory?.SubCategoryNameUrlEN}/{targetProduct.ProductNameUrlEN}";
                    }
                    else
                    {
                        dto.PhotoAltTag = targetProduct.PhotoAltTagEN;
                        dto.SliderHref =
                             $"/{dto.Culture}/{prodeta}/{targetProduct.Brand.BandNameUrl}/{targetProduct.TopCategory.TopCategoryNameUrlEN}/{targetProduct.MiddleCategory?.MiddleCategoryNameUrlEN}/{targetProduct.SubCategory?.SubCategoryNameUrlEN}/{targetProduct.ProductNameUrlEN}";
                    }
                    dto.SliderHref = dto.SliderHref.Replace("///", "/", StringComparison.Ordinal);
                    dto.SliderHref = dto.SliderHref.Replace("//", "/", StringComparison.Ordinal);

                    var miniSliderDb = _mapper.Map<MiniSlider>(dto);
                    bool a = _miniSliderRepo.UpdateEntity(miniSliderDb);

                    bool b = true;

                    if (dto.SliderPhoto != null)
                    {
                        b = ProcessMiniSliderPhoto(dto.SliderPhoto, miniSliderDb.Id);
                    }
                    if (a && b)
                    {
                        HttpContext.Session.SetString("success", "Mini Slider Güncelleme işlemi başarılı.");
                        return RedirectToAction("MiniSliderList");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", "Mini Slider Güncellenirken hata oluştu. Hata kayıt altına alındı.");

                        _errorRepo.CreateEntity(new ErrorLog
                        {
                            Culture = "tr",
                            ErrorLocation = "Admin MiniSlider UpdateMiniSlider satır2294",
                            ErrorDetail = $"a:{a}, b:{b}",
                            ErrorUrl = HttpContext.Request.Path,
                        });
                        return RedirectToAction("MiniSliderList");
                    }
                }

                HttpContext.Session.SetString("warning", $"{messages}, Lütfen formu eksiksiz doldurunuz.");
                return RedirectToAction("MiniSliderList");
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException + " " + messages,
                    ErrorLocation = "MiniSliderController UpdateMiniSlider",
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("warning", "MiniSliderList güncelleme işleminde hata oluştu.");
                return RedirectToAction("MiniSliderList");

            }
        }

        public bool ProcessMiniSliderPhoto(IFormFile file, int miniSliderId)
        {
            try
            {
                //eğer daha önceden mini slider photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var miniSliderDb = _miniSliderRepo.GetEntityById(miniSliderId);

                if (!string.IsNullOrEmpty(miniSliderDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "MiniSliderImages", miniSliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, miniSliderDb.PhotoFileName);

                        if (System.IO.File.Exists(fullPathToDelete))
                        {
                            System.IO.File.Delete(fullPathToDelete);
                        }
                }

                var uniqueFileName = GetUniqueMiniSliderPhotoFileName(file.FileName, miniSliderId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "MiniSliderImages", "Raw");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "MiniSliderImages", miniSliderDb.Culture);
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
                var imageModified = new Bitmap(280, 355);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 280, 355);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 280, 355);
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 280, 355);
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                miniSliderDb.PhotoFileName = uniqueFileName;
                bool dd = _miniSliderRepo.UpdateEntity(miniSliderDb);

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
                    ErrorLocation = "Admin MiniSlider ProcessMiniSliderPhoto try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }
        }

        public JsonResult DeleteMiniSlider(int miniSliderId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var miniSliderDb = _miniSliderRepo.GetEntityById(miniSliderId);

                string fullPathMain = Path.Combine(_environment.WebRootPath, "Images", "MiniSliderImages", miniSliderDb.Culture, miniSliderDb.PhotoFileName);

                if (System.IO.File.Exists(fullPathMain))
                {
                    System.IO.File.Delete(fullPathMain);
                }

                miniSliderDb.PhotoFileName = null;
                bool a = _miniSliderRepo.UpdateEntity(miniSliderDb);

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli MiniSlider DeleteMiniSlider",
                        ErrorDetail = "Mini Slider PhotoFileName bilgisi güncellenemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Mini Slider PhotoFileName bilgisi güncellenemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Mini Slider fotoğrafı silindi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli Mini Slider DeleteMiniSlider",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Mini Slider Fotoğrafı silinemedi. Hata oluştu. {e}" });
            }
        }

        private string GetUniqueMiniSliderPhotoFileName(string fileName, int miniSliderId)
        {
            return "MiniSliderId-" + miniSliderId
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        public JsonResult HardDeleteMiniSlider(int id)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var miniSliderDb = _miniSliderRepo.GetEntityById(id);

                if (!string.IsNullOrEmpty(miniSliderDb.PhotoFileName))
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "MiniSliderImages", miniSliderDb.Culture);
                    string fullPathToDelete = Path.Combine(folderPathToDelete, miniSliderDb.PhotoFileName);

                    if (System.IO.File.Exists(fullPathToDelete))
                    {
                        System.IO.File.Delete(fullPathToDelete);
                    }
                }

                bool a = _miniSliderRepo.HardDeleteEntity(miniSliderDb);

                if (a)
                {
                    return Json(new { success = true, message = "Mini Slider kalıcı olarak silindi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Mini Slider silinemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin MiniSlider HardDeleteMiniSlider try catch",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return Json(new { success = false, message = "Mini Slider silinemedi. Hata oluştu." });
            }

        }

        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }

    }
}
