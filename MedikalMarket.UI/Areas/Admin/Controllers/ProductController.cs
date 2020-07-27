using AutoMapper;
using MedikalMarket.UI.Areas.Admin.Dtos;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
    public class ProductController : Controller
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductPhotoRepository _photoRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFavoriteProductRepository _fpRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ITopCategoryRepository topRepo, IProductRepository productRepo, IErrorLogRepository errorRepo, IHttpContextAccessor httpContextAccessor, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IFavoriteProductRepository fpRepo, ICustomerRepository customerRepo, IBrandRepository brandRepo, IMapper mapper, IWebHostEnvironment environment, IProductPhotoRepository photoRepo)
        {
            _topRepo = topRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _httpContextAccessor = httpContextAccessor;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _fpRepo = fpRepo;
            _customerRepo = customerRepo;
            _brandRepo = brandRepo;
            _mapper = mapper;
            _environment = environment;
            _photoRepo = photoRepo;
        }

        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            return View();
        }

        public async Task<IActionResult> ProductList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");
            var products = await Task.Run(() => _productRepo.GetActiveProductsWithJoin().ToList()).ConfigureAwait(false);
            var proDto = await Task.Run(() => ProDtoMapper(products.ToList())).ConfigureAwait(false);
            return View(proDto);
        }

        public async Task<IActionResult> DeletedProductList()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");
            var products = await Task.Run(() => _productRepo.GetDeletedProductsWithJoin().ToList()).ConfigureAwait(false);
            var proDto = await Task.Run(() => ProDtoMapper(products.ToList())).ConfigureAwait(false);
            return View(proDto);
        }

        [HttpGet]
        public IActionResult EditProductPopup(int id)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            var productDb = _productRepo.GetProductWithId(id) ?? _productRepo.GetDeactiveProductWithId(id);
            var dto = new AdminEditProductDto
            {
                Id = productDb.Id,
                DiscountRate = productDb.DiscountRate,
                HasNewBadge = productDb.HasNewBadge,
                HeadDescriptionTR = productDb.HeadDescriptionTR,
                HeadDescriptionEN = productDb.HeadDescriptionEN,
                HeadDescriptionRU = productDb.HeadDescriptionRU,
                HeadTitleTR = productDb.HeadTitleTR,
                HeadTitleEN = productDb.HeadTitleEN,
                HeadTitleRU = productDb.HeadTitleRU,
                IsFreeShipping = productDb.IsFreeShipping,
                MainPhotoFileName = productDb.MainPhotoFileName,
                PhotoAltTagTR = productDb.PhotoAltTagTR,
                PhotoAltTagEN = productDb.PhotoAltTagEN,
                PhotoAltTagRU = productDb.PhotoAltTagRU,
                TopCategories = _topRepo.FindEntities(x=>!x.IsDeleted).OrderBy(x=>x.NameTR).ToList(),
                MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted && x.TopCategoryId==productDb.TopCategoryId).OrderBy(x => x.NameTR).ToList(),
                SubCategories = _subRepo.FindEntities(x => !x.IsDeleted && x.MiddleCategoryId==productDb.MiddleCategoryId).OrderBy(x => x.NameTR).ToList(),
                Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList(),
                NameTR = productDb.NameTR,
                NameEN = productDb.NameEN,
                NameRU = productDb.NameRU,
                NumberInStock = productDb.NumberInStock,
                ProductCode = productDb.ProductCode,
                ProductDescriptionTR = productDb.ProductDescriptionTR,
                ProductDescriptionEN = productDb.ProductDescriptionEN,
                ProductDescriptionRU = productDb.ProductDescriptionRU,
                SelectedProductOfferType = (int)productDb.ProductOfferType,
                SelectedBrandId = productDb.BrandId,
                SelectedMiddleCategoryId = productDb.MiddleCategoryId ?? 0,
                SelectedSubCategoryId = productDb.SubCategoryId ?? 0,
                SelectedTopCategoryId = productDb.TopCategoryId,
                StockNumber = productDb.StockNumber
            };
            dto.AdminProductPhotoDtos = new List<AdminProductPhotoDto>();

            foreach (var item in productDb.ProductPhotos)
            {
                var photoDto = new AdminProductPhotoDto
                {
                    Id = item.Id,
                    IsMainPhoto = item.IsMainPhoto,
                    PhotoFileName = item.PhotoFileName
                };
                dto.AdminProductPhotoDtos.Add(photoDto);
            }

            return PartialView(dto);

        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            ViewBag.success = HttpContext.Session.GetString("success");
            ViewBag.warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("success");
            HttpContext.Session.Remove("warning");
            var dto = new AdminCreateProductDto
            {
                Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x=>x.BrandName).ToList(),
                TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList(),
                MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList(),
                SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList(),
                SelectedProductOfferType = (int)ProductOfferType.Nothing
            };
            dto.AdminProductPhotoDtos = new List<AdminProductPhotoDto>();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(AdminCreateProductDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            string messages = string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));

            try
            {
                //seçilen marka silnimiş ise hata döner
                if (_brandRepo.GetEntityById(dto.SelectedBrandId).IsDeleted)
            {
                ModelState.AddModelError("warning", $"Seçilen marka ({_brandRepo.GetEntityById(dto.SelectedBrandId).BrandName}) pasif ya da silinmiş durumdadır. Lütfen önce markayı aktive ediniz veya başka bir marka seçiniz.");
                dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }
            //seçilen ana kategori pasif ya da silnimiş ise hata döner
            if (_topRepo.GetEntityById(dto.SelectedTopCategoryId).IsDeleted)
            {
                ModelState.AddModelError("warning", $"Seçilen ana kategori ({_topRepo.GetEntityById(dto.SelectedTopCategoryId).NameTR})pasif ya da silinmiş durumdadır. Lütfen önce ana kategoriyi aktive ediniz veya başka bir ana kategori seçiniz.");
                dto.Brands = _brandRepo.FindEntities(x=>!x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }
            //seçilen orta kategori pasif ya da silnimiş ise hata döner
            int midCateId = dto.SelectedMiddleCategoryId ?? 0;
            if (midCateId != 0 && _midRepo.GetEntityById((int)dto.SelectedMiddleCategoryId).IsDeleted)
            {
                ModelState.AddModelError("warning", $"Seçilen orta kategori ({_midRepo.GetEntityById((int)dto.SelectedMiddleCategoryId).NameTR}) silinmiş durumdadır. Lütfen önce orta kategoriyi aktive ediniz veya başka bir orta kategori seçiniz.");
                dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }

            //seçilen alt kategori pasif ya da silnimiş ise hata döner
            int subCateId = dto.SelectedSubCategoryId ?? 0;

            if (subCateId != 0 && _subRepo.GetEntityById((int)dto.SelectedSubCategoryId).IsDeleted)
            {
                ModelState.AddModelError("warning", $"Seçilen alt kategori ({_subRepo.GetEntityById((int)dto.SelectedSubCategoryId).NameTR}) pasif ya da silinmiş durumdadır. Lütfen önce alt kategoriyi aktive ediniz veya başka bir alt kategori seçiniz.");
                dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }

            IEnumerable<object> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

            if (dto.MainPhoto != null)
            {
                FileInfo fs = new FileInfo(dto.MainPhoto.FileName);
                string ext = Path.GetExtension(fs.Extension);

                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    ModelState.AddModelError("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                    dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                    dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                    dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                    dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                    dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                    return View("CreateProduct", dto);
                }

            }
            if (dto.AdditionalPhotos != null)
            {
                foreach (var item in dto.AdditionalPhotos)
                {
                    FileInfo fs2 = new FileInfo(item.FileName);
                    string ext2 = Path.GetExtension(fs2.Extension);

                    if (!allowedExtensions.Contains(ext2.ToLower()))
                    {
                        ModelState.AddModelError("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                        dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                        dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                        dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                        dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                        return View("CreateProduct", dto);
                    }
                }
            }

            var proDb = new Product();
                string brandName = _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName;
                if (ModelState.IsValid)
                {
                    proDb.NameTR = dto.NameTR;
                    proDb.NameEN = dto.NameEN;
                    proDb.NameRU = dto.NameRU;
                    proDb.ProductNameUrlTR = dto.NameTR.ConvertToFriendlyUrl();
                    proDb.ProductNameUrlEN = dto.NameEN.ConvertToFriendlyUrl();
                    proDb.ProductNameUrlRU = dto.NameEN.ConvertToFriendlyUrl();
                    proDb.ProductDescriptionTR = dto.ProductDescriptionTR;
                    proDb.ProductDescriptionEN = dto.ProductDescriptionEN;
                    proDb.ProductDescriptionRU = dto.ProductDescriptionRU;
                    proDb.ProductCode = dto.ProductCode;
                    proDb.DiscountRate = dto.DiscountRate;
                    proDb.StockNumber = dto.StockNumber;
                    proDb.NumberInStock = dto.NumberInStock;
                    proDb.ProductOfferType = (ProductOfferType)dto.SelectedProductOfferType;
                    proDb.HasNewBadge = dto.HasNewBadge;
                    proDb.IsFreeShipping = dto.IsFreeShipping;

                    proDb.HeadTitleTR = dto.NameTR + "-" + $"{brandName} marka ürünler, premium medikal market";
                    proDb.HeadTitleEN = dto.NameEN + "-" + $"{brandName} products, premium medical market";
                    proDb.HeadTitleRU = dto.NameRU + "-" + $"продукты бренда { brandName}, премиум медицинский рынок";

                    string midCateNameTR = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameTR : null;
                    string midCateNameEN = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameEN : null;
                    string midCateNameRU = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameRU : null;

                    string subCateNameTR = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameTR : null;
                    string subCateNameEN = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameEN : null;
                    string subCateNameRU = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameRU : null;

                    proDb.HeadDescriptionTR = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameTR + "-" + midCateNameTR + "-" + subCateNameTR + "-" + dto.NameTR;

                    proDb.HeadDescriptionEN = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameEN + "-" + midCateNameEN + "-" + subCateNameEN + "-" + dto.NameEN;

                    proDb.HeadDescriptionRU = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameRU + "-" + midCateNameRU + "-" + subCateNameRU + "-" + dto.NameRU;

                    proDb.PhotoAltTagTR = _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " Markasına ait " + dto.NameTR + " Ürün Fotoğrafı " +  "premiummedikal.com";

                    proDb.PhotoAltTagEN= _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " Brand " + dto.NameEN + " Product Photo " + "premiummedikal.com";
                    
                    proDb.PhotoAltTagRU= _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " марка " + dto.NameEN + " Фото продукта " + "premiummedikal.com";
                    
                    proDb.BrandId = dto.SelectedBrandId;
                    proDb.TopCategoryId = dto.SelectedTopCategoryId;
                    proDb.MiddleCategoryId = dto.SelectedMiddleCategoryId;
                    proDb.SubCategoryId = dto.SelectedSubCategoryId;
                    bool aa = _productRepo.CreateEntity(proDb);
                    bool bb = true;
                    bool cc = true;

                    if (dto.MainPhoto != null)
                    {
                        bb = ProcessMainPhoto(dto.MainPhoto, proDb.Id);
                    }
                    if (dto.AdditionalPhotos != null)
                    {
                        cc = ProcessAdditionalPhotos(dto.AdditionalPhotos, proDb.Id);
                    }
                    if (aa && bb && cc)
                    {
                        HttpContext.Session.SetString("success", "Ürün ekleme işlemi başarılı.");
                        return RedirectToAction("CreateProduct");
                    }
                    else
                    {
                        HttpContext.Session.SetString("warning", $"Ürün ekleme işlemi:{aa}, Vitrin fotoğrafı ekleme işlemi: {bb}, Detay sayfası gotoğrafları ekleme işlemi: {cc}.");
                        return RedirectToAction("CreateProduct");
                    }
                }
               
                ModelState.AddModelError("warning", $"Ürün eklenemedi. Hata oluştu. Lütfen formu tam doldurunuz. {messages}");

                dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }

            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "ProductController CreateProduct",
                    ErrorDetail = e.Message + " , " + e.InnerException + " " + "ürün ekleme try catch, " + messages,
                    ErrorUrl = HttpContext.Request.Path,
                });

                ModelState.AddModelError("warning", $"Ürün eklenemedi. Hata oluştu. Lütfen formu tam doldurunuz. {messages}");
                dto.Brands = _brandRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.BrandName).ToList();
                dto.TopCategories = _topRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.MiddleCategories = _midRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SubCategories = _subRepo.FindEntities(x => !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
                dto.SelectedProductOfferType = (int)ProductOfferType.Nothing;
                return View("CreateProduct", dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(AdminEditProductDto dto)
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }
            String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
            try
            {
                IEnumerable<object> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

            if (dto.MainPhoto != null)
            {
                FileInfo fs = new FileInfo(dto.MainPhoto.FileName);
                string ext = Path.GetExtension(fs.Extension);

                if (!allowedExtensions.Contains(ext.ToLower()))
                {
                    HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                    return RedirectToAction("ProductList");
                }

            }
            if (dto.AdditionalPhotos != null)
            {
                foreach (var item in dto.AdditionalPhotos)
                {
                    FileInfo fs2 = new FileInfo(item.FileName);
                    string ext2 = Path.GetExtension(fs2.Extension);

                    if (!allowedExtensions.Contains(ext2.ToLower()))
                    {
                        HttpContext.Session.SetString("warning", "Lütfen sadece .jpg, .jpeg, .bmp veya .png uzantılı resim yükleyiniz.");
                        return RedirectToAction("ProductList");
                    }
                }
            }

            var proDb = _productRepo.GetProductWithId(dto.Id) ?? _productRepo.GetDeactiveProductWithId(dto.Id);
                int midCateId = dto.SelectedMiddleCategoryId ?? 0;
                int subCateId = dto.SelectedSubCategoryId ?? 0;


                string midCateNameTR = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameTR : null;
                string midCateNameEN = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameEN : null;
                string midCateNameRU = midCateId != 0 ? _midRepo.GetEntityById(midCateId).NameRU : null;

                string subCateNameTR = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameTR : null;
                string subCateNameEN = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameEN : null;
                string subCateNameRU = subCateId != 0 ? _subRepo.GetEntityById(subCateId).NameRU : null;

                if (ModelState.IsValid)
                {
                    proDb.NameTR = dto.NameTR;
                    proDb.NameEN = dto.NameEN;
                    proDb.NameRU = dto.NameRU;
                    proDb.ProductNameUrlTR = dto.NameTR.ConvertToFriendlyUrl();
                    proDb.ProductNameUrlEN = dto.NameEN.ConvertToFriendlyUrl();
                    proDb.ProductNameUrlRU = dto.NameEN.ConvertToFriendlyUrl();
                    proDb.ProductDescriptionTR = dto.ProductDescriptionTR;
                    proDb.ProductDescriptionEN = dto.ProductDescriptionEN;
                    proDb.ProductDescriptionRU = dto.ProductDescriptionRU;
                    proDb.ProductCode = dto.ProductCode;
                    proDb.DiscountRate = dto.DiscountRate;
                    proDb.StockNumber = dto.StockNumber;
                    proDb.NumberInStock = dto.NumberInStock;
                    proDb.ProductOfferType = (ProductOfferType)dto.SelectedProductOfferType;
                    proDb.HasNewBadge = dto.HasNewBadge;
                    proDb.IsFreeShipping = dto.IsFreeShipping;
                    proDb.PhotoAltTagRU = dto.PhotoAltTagRU;
                    proDb.BrandId = dto.SelectedBrandId;
                    proDb.TopCategoryId = dto.SelectedTopCategoryId;
                    proDb.MiddleCategoryId = dto.SelectedMiddleCategoryId;
                    proDb.SubCategoryId = dto.SelectedSubCategoryId;
                    proDb.HeadTitleTR = dto.NameTR + "-" + " premium medikal market";
                    proDb.HeadTitleEN = dto.NameEN + "-" + " premium medical market";
                    proDb.HeadTitleRU = dto.NameRU + "-" + " премиум медицинский рынок";
                    proDb.HeadDescriptionTR = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameTR + "-" + midCateNameTR + "-" + subCateNameTR + "-" + dto.NameTR;

                    proDb.HeadDescriptionEN = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameEN + "-" + midCateNameEN + "-" + subCateNameEN + "-" + dto.NameEN;

                    proDb.HeadDescriptionRU = _topRepo.GetEntityById(dto.SelectedTopCategoryId).NameRU + "-" + midCateNameRU + "-" + subCateNameRU + "-" + dto.NameRU;

                    proDb.PhotoAltTagTR = _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " Markasına ait " + dto.NameTR + " Ürün Fotoğrafı " + "premiummedikal.com";

                    proDb.PhotoAltTagEN = _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " Brand " + dto.NameEN + " Product Photo " + "premiummedikal.com";

                    proDb.PhotoAltTagRU = _brandRepo.GetEntityById(dto.SelectedBrandId).BrandName + " марка " + dto.NameEN + " Фото продукта " + "premiummedikal.com";

                    bool aa = _productRepo.UpdateEntity(proDb);
                    bool bb = true;
                    bool cc = true;

                    if (dto.MainPhoto != null)
                    {
                        bb = ProcessMainPhoto(dto.MainPhoto, proDb.Id);
                    }
                    if (dto.AdditionalPhotos != null)
                    {
                        cc = ProcessAdditionalPhotos(dto.AdditionalPhotos, proDb.Id);
                    }
                    if (aa && bb && cc)
                    {
                        if (!proDb.IsDeleted)
                        {
                            HttpContext.Session.SetString("success", "Ürün bilgileri başarıyla güncellendi.");
                            return RedirectToAction("ProductList");
                        }
                        else
                        {
                            HttpContext.Session.SetString("success", "Ürün bilgileri başarıyla güncellendi.");
                            return RedirectToAction("DeletedProductList");
                        }

                    }
                }

                if (!proDb.IsDeleted)
                {
                    HttpContext.Session.SetString("warning", $"Ürün güncellenemedi. Hata oluştu. Lütfen formu tam doldurunuz. {messages}");
                    return RedirectToAction("ProductList");
                }
                else
                {
                    HttpContext.Session.SetString("warning", $"Ürün güncellenemedi. Hata oluştu. Lütfen formu tam doldurunuz. {messages}");
                    return RedirectToAction("DeletedProductList");
                }
            }

            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "UpdateProduct",
                    ErrorDetail = e.Message + " , " + e.InnerException + " " + "ürün güncelleme try catch," + messages,
                    ErrorUrl = HttpContext.Request.Path,
                });

                    HttpContext.Session.SetString("warning", "Ürün güncellenemedi. Hata oluştu. Hata kayıt altına alındı");
                    return RedirectToAction("ProductList");
            }
        }

        private string GetUniqueFileName(string fileName, int productId)
        {
            var compId = _productRepo.GetProductWithId(productId).BrandId;
            return "ProId-" + productId + "_CompId-" + compId
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        public bool ProcessMainPhoto(IFormFile file, int productId)
        {

            try
            {
                //eğer daha önceden main photo olarak atanmış foto var ve yeni yüklenen de varsa eskileri bulup siliyorum ki çakışma olmasın
                var proDb = _productRepo.GetEntityById(productId);
                var mainPhotos = _photoRepo.FindEntities(x => x.ProductId == productId && x.IsMainPhoto).ToList();
                bool aa = true;

                if (mainPhotos.Count > 0)
                {
                    string folderPathToDelete = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Main");
                    foreach (var item in mainPhotos)
                    {
                        string fullPathToDelete = Path.Combine(folderPathToDelete, item.PhotoFileName);
                        if (System.IO.File.Exists(fullPathToDelete))
                        {
                            System.IO.File.Delete(fullPathToDelete);
                        }
                    }
                    aa = _photoRepo.HardDeleteGivenEntities(mainPhotos);
                }

                var uniqueFileName = GetUniqueFileName(file.FileName, productId);
                string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Raw", "Main");
                string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Main");
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
                var imageModified = new Bitmap(250, 250);
                imageModified.Save(fullPathModified);
                imageModified.Dispose();
                //var imageRaw = Image.FromFile(fullPathRaw);
                //imageRaw.Dispose();

                bool bb;
                switch (fs.Extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 250, 250);
                        //imageRaw.Dispose();
                        //imageModified.Dispose();
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    case ".bmp":
                    case ".png":
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 250, 250);

                        //Image vv = ImageHelper.GetCompressedBitmap((Bitmap)imageRaw, 60L);
                        //var gg = Graphics.FromImage(vv);
                        //gg.DrawImage(imageRaw, 0, 0, 250, 250);
                        //imageModified.Dispose();
                        //System.IO.File.Delete(fullPathModified);
                        //string fullPath3 = Path.Combine(folderPathModified, fs.Name);
                        //vv.Save(fullPath3);
                        //vv.Dispose();
                        //gg.Dispose();
                        //bb = true;
                        //imageRaw.Dispose();
                        //imageModified.Dispose();
                        System.IO.File.Delete(fullPathRaw);
                        break;

                    default:
                        bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 250, 250);
                        //imageRaw.Dispose();
                        //imageModified.Dispose();
                        System.IO.File.Delete(fullPathRaw);
                        break;
                }

                bool cc = _photoRepo.CreateEntity(new ProductPhoto
                {
                    IsMainPhoto = true,
                    PhotoFileName = uniqueFileName,
                    ProductId = productId
                });

                proDb.MainPhotoFileName = uniqueFileName;
                bool dd = _productRepo.UpdateEntity(proDb);
                if (aa && bb && cc && dd)
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
                    ErrorLocation = "Admin Product Controller ProcessMainPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });

                return false;
            }

            //return null;
        }

        public bool ProcessAdditionalPhotos(List<IFormFile> formFiles, int productId)
        {
            try
            {
                foreach (var file in formFiles)
                {
                    var uniqueFileName = GetUniqueFileName(file.FileName, productId);
                    string folderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Raw", "Additional");
                    string fullPathRaw = Path.Combine(folderPathRaw, uniqueFileName);
                    string folderPathModified = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Additional");
                    string fullPathModified = Path.Combine(folderPathModified, uniqueFileName);

                    string thumbFolderPathRaw = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Raw", "Thumb");
                    string thumbFullPathRaw = Path.Combine(thumbFolderPathRaw, uniqueFileName);
                    string thumbFolderPathModified = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Thumb");
                    string thumbFullPathModified = Path.Combine(thumbFolderPathModified, uniqueFileName);


                    if (!Directory.Exists(folderPathRaw))
                    {
                        Directory.CreateDirectory(folderPathRaw);
                    }
                    if (!Directory.Exists(folderPathModified))
                    {
                        Directory.CreateDirectory(folderPathRaw);
                    }

                    using (var fileStream = new FileStream(fullPathRaw, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    FileInfo fs = new FileInfo(fullPathRaw);
                    var imageModified = new Bitmap(250, 250);
                    imageModified.Save(fullPathModified);
                    imageModified.Dispose();
                    //var imageRaw = Image.FromFile(fullPathRaw);
                    //imageRaw.Dispose();
                    //imageModified.Dispose();

                    var imageModified2 = new Bitmap(85, 85);
                    imageModified2.Save(thumbFullPathModified);
                    imageModified2.Dispose();
                    //var imageRaw2 = Image.FromFile(fullPathRaw);
                    //imageRaw2.Dispose();
                    //imageModified2.Dispose();

                    bool bb;
                    bool bb2;

                    switch (fs.Extension)
                    {
                        case ".jpeg":
                        case ".jpg":
                            bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 500, 500);
                            bb2 = ImageHelper.ResizeAndCompressImage(fullPathRaw, thumbFullPathModified, ImageFormat.Jpeg, 85, 85);
                            //imageRaw.Dispose();
                            //imageModified.Dispose();
                            //imageRaw2.Dispose();
                            //imageModified2.Dispose();
                            System.IO.File.Delete(fullPathRaw);
                            break;

                        case ".bmp":
                        case ".png":

                            //var bitmap = new Bitmap(Image.FromFile(fullPathRaw));
                            //Image uploadedImage = Image.FromFile(fullPathRaw);
                            //System.IO.File.Delete(fullPathRaw);
                            //bitmap.Save(fullPathRaw, ImageFormat.Jpeg);

                            bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 500, 500);
                            bb2 = ImageHelper.ResizeAndCompressImage(fullPathRaw, thumbFullPathModified, ImageFormat.Jpeg, 85, 85);
                            //imageRaw.Dispose();
                            //imageModified.Dispose();
                            //imageRaw2.Dispose();
                            //imageModified2.Dispose();
                            System.IO.File.Delete(fullPathRaw);


                            //Image vv = ImageHelper.GetCompressedBitmap((Bitmap)imageRaw, 60L);
                            //var gg = Graphics.FromImage(vv);
                            //gg.DrawImage(imageRaw, 0, 0, 500, 500);
                            //imageModified.Dispose();
                            //System.IO.File.Delete(fullPathModified);
                            //string fullPath3 = Path.Combine(folderPathModified, fs.Name);
                            //vv.Save(fullPath3);
                            //vv.Dispose();
                            //gg.Dispose();
                            //bb = true;
                            //imageRaw.Dispose();
                            //imageModified.Dispose();
                            //System.IO.File.Delete(fullPathRaw);
                            break;

                        default:
                            bb = ImageHelper.ResizeAndCompressImage(fullPathRaw, fullPathModified, ImageFormat.Jpeg, 500, 500);
                            bb2 = ImageHelper.ResizeAndCompressImage(fullPathRaw, thumbFullPathModified, ImageFormat.Jpeg, 85, 85);
                            //imageRaw.Dispose();
                            //imageModified.Dispose();
                            //imageRaw2.Dispose();
                            //imageModified2.Dispose();
                            System.IO.File.Delete(fullPathRaw);
                            break;
                    }

                    bool cc = _photoRepo.CreateEntity(new ProductPhoto
                    {
                        IsMainPhoto = false,
                        PhotoFileName = uniqueFileName,
                        ProductId = productId
                    });

                    if (!bb || !cc || !bb2)
                    {
                        return false;
                    }
                }

                return true;
            }

            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Product Controller ProcessAdditionalPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                });
                return false;
            }
        }

        public JsonResult DeleteProduct(int id)
        {
            try
            {
                var product = _productRepo.GetEntityById(id);
                product.IsDeleted = true;
                bool a = _productRepo.UpdateEntity(product);
                if (a)
                {
                    return Json(new { success = true, message = "Ürün silindi.", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Ürün bilgisi güncellenemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException,
                    ErrorLocation = "ProductController DeleteProduct",
                    ErrorUrl = HttpContext.Request.Path
                });
                return Json(new { success = false, message = "Ürün bilgisi güncellenemedi. Hata oluştu." });
            }

        }

        public JsonResult UnDeleteProduct(int id)
        {
            try
            {
                var product = _productRepo.GetEntityById(id);
                product.IsDeleted = false;
                bool a = _productRepo.UpdateEntity(product);
                if (a)
                {
                    return Json(new { success = true, message = "Ürün silme işlemi geri alındı. Pasif ürün listesine eklendi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Ürün bilgisi güncellenemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException,
                    ErrorLocation = "ProductController UndeleteProduct",
                    ErrorUrl = HttpContext.Request.Path
                });
                return Json(new { success = false, message = "Ürün bilgisi güncellenemedi. Hata oluştu." });
            }

        }

        public JsonResult HardDeleteProduct(int id)
        {
            try
            {
                var product = _productRepo.GetEntityById(id);

                var photos = _photoRepo.FindEntities(x => x.ProductId == product.Id);

                string folderPathMain = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Main");
                string folderPathAdditional = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Additional");
                string folderPathThumb = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Thumb");


                foreach (var item in photos)
                {
                    string fullPathMain = Path.Combine(folderPathMain, item.PhotoFileName);
                    string fullPathAdditional = Path.Combine(folderPathAdditional, item.PhotoFileName);
                    string fullPathThumb = Path.Combine(folderPathThumb, item.PhotoFileName);

                    if (System.IO.File.Exists(fullPathMain))
                    {
                        System.IO.File.Delete(fullPathMain);
                    }
                    if (System.IO.File.Exists(fullPathAdditional))
                    {
                        System.IO.File.Delete(fullPathAdditional);
                    }
                    if (System.IO.File.Exists(fullPathThumb))
                    {
                        System.IO.File.Delete(fullPathThumb);
                    }
                }

                bool a = _productRepo.HardDeleteEntity(product);

                if (a)
                {
                    return Json(new { success = true, message = "Ürün kalıcı olarak silindi", proId = id });
                }
                else
                {
                    return Json(new { success = false, message = "Ürün silinemedi." });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message + " " + e.InnerException,
                    ErrorLocation = "ProductController HardDeleteProduct",
                    ErrorUrl = HttpContext.Request.Path
                });
                return Json(new { success = false, message = "Ürün silinemedi. Hata oluştu." });
            }

        }

        public IActionResult ValidateProductNameTR(string NameTR)
        {
            var productDb = _productRepo.GetAllEntities().Where(x=>x.NameTR.Equals(NameTR)).ToList();
            if (productDb.Count == 0)
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult ValidateProductNameEN(string NameEN)
        {
            var productDb = _productRepo.GetAllEntities().Where(x => x.NameTR.Equals(NameEN)).ToList();
            if (productDb.Count == 0)
            {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult ValidateProductNameRU(string NameRU)
        {
            var productDb = _productRepo.GetAllEntities().Where(x => x.NameTR.Equals(NameRU)).ToList();
            if (productDb.Count == 0)
            {
                return Json(true);
            }
            return Json(false);
        }

        public async Task<IActionResult> ProductList2()
        {
            var filePath = Path.Combine(_environment.WebRootPath);
            string fileName = "Products.Json";
            string fullPath = Path.Combine(filePath, fileName);

            if (!System.IO.File.Exists(fullPath))
            {

                var products = await Task.Run(() => _productRepo.GetActiveProductsWithJoin().ToList()).ConfigureAwait(false);
                var dtoList = new List<AdminProductDto>();
                foreach (var item in products)
                {
                    dtoList.Add(new AdminProductDto
                    {
                        Id = item.Id,
                        MainPhotoFileName = item.MainPhotoFileName,
                        PhotoAltTagTR = item.PhotoAltTagTR,
                        ProductOfferType = item.ProductOfferType,
                        NameTR = item.NameTR,
                        BrandName = item.Brand.BrandName,
                        MiddleCategoryName = item.MiddleCategory?.NameTR,
                        SubCategoryName = item.SubCategory?.NameTR,
                        TopCategoryName = item.TopCategory.NameTR
                    });
                }

                var jsonObj = JsonConvert.SerializeObject(dtoList);
                //JObject obj = (JObject)JToken.FromObject(proDto);

                System.IO.File.WriteAllText(fullPath, jsonObj);
            }

            return View();
        }

        public List<AdminProductDto> ProDtoMapper(List<Product> proList)
        {
            var dtoList = new List<AdminProductDto>();

            foreach (var item in proList)
            {
                dtoList.Add(new AdminProductDto
                {
                    Id = item.Id,
                    MainPhotoFileName = item.MainPhotoFileName,
                    PhotoAltTagTR = item.PhotoAltTagTR,
                    ProductOfferType = item.ProductOfferType,
                    NameTR = item.NameTR,
                    BrandName = item.Brand.BrandName,
                    MiddleCategoryName = item.MiddleCategory?.NameTR,
                    SubCategoryName = item.SubCategory?.NameTR,
                    TopCategoryName = item.TopCategory.NameTR
                });
            }

            return dtoList;
        }

        public JsonResult GetMiddleCategoryList(int id)
        {
            var midCategories = _midRepo.FindEntities(x => x.TopCategoryId == id && !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            return Json(new SelectList(midCategories, "Id", "NameTR"));
        }

        public JsonResult GetSubCategoryList(int id)
        {
            var subCategories = _subRepo.FindEntities(x => x.MiddleCategoryId == id && !x.IsDeleted).OrderBy(x => x.NameTR).ToList();
            return Json(new SelectList(subCategories, "Id", "NameTR"));
        }

        public JsonResult DeleteMainPhoto(int photoId, int proId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var photoDb = _photoRepo.GetEntityById(photoId);
                var productDb = _productRepo.GetEntityById(proId);

                string photoFileName = photoDb.PhotoFileName;

                string fullPath = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Main", photoDb.PhotoFileName);

                bool a = _photoRepo.HardDeleteEntity(photoDb);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                productDb.MainPhotoFileName = null;
                bool b = _productRepo.UpdateEntity(productDb);

                if (!a && !b)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli DeleteMainPhoto",
                        ErrorDetail = $"{proId} id ye sahip ürünün mainphotofilename propertysi güncellnirken hata oluştu. Ayrıca {photoId} id sine sahip fotoğraf database'den silinemedi.",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Vitrin fotoğrafı silindi." });
                }
                else if (a && !b)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli DeleteMainPhoto",
                        ErrorDetail = $"{proId} id ye sahip ürünün mainphotofilename propertysi güncellnirken hata oluştu",
                        ErrorUrl = HttpContext.Request.Path
                    });

                    return Json(new { success = false, message = "Vitrin fotoğrafı silindi ancak ürünün vitrin fotoğraf bilgisi güncellenemedi. Lütfen Yeni vitrin fotoğrafı yükleyiniz." });
                }

                else if (!a && b)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli DeleteMainPhoto",
                        ErrorDetail = $"{photoId} id ye sahip fotoğraf database'den silinemedi. ",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return Json(new { success = false, message = "Vitrin fotoğrafı fiziksel olarak silindi database'den silinirken hata oluştu." });
                }

                else
                {
                    return Json(new { success = true, message = "Vitrin fotoğrafı silindi." });
                }


            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli DeleteMainPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = $"Fotoğraf silinemedi. Hata oluştu. {e}" });
            }
        }

        public JsonResult DeleteAdditionalPhoto(int photoId)
        {
            try
            {
                if (!AuthCheck())
                {
                    return Json(new { success = false, message = "Oturumunuz sonlanmış. Lütfen login olunuz." });
                }
                var photoDb = _photoRepo.GetEntityById(photoId);

                string photoFullPath = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Additional", photoDb.PhotoFileName);
                string thumbFullPath = Path.Combine(_environment.WebRootPath, "Images", "ProductImages", "Thumb", photoDb.PhotoFileName);

                bool a = _photoRepo.HardDeleteEntity(photoDb);

                if (System.IO.File.Exists(photoFullPath))
                {
                    System.IO.File.Delete(photoFullPath);
                }

                if (System.IO.File.Exists(thumbFullPath))
                {
                    System.IO.File.Delete(thumbFullPath);
                }

                if (!a)
                {
                    _errorRepo.CreateEntity(new ErrorLog
                    {
                        Culture = "tr",
                        ErrorLocation = "Admin Paneli DeleteAdditionalPhoto",
                        ErrorDetail = $"{photoId} id ye sahip fotoğraf database'den silinemedi. ",
                        ErrorUrl = HttpContext.Request.Path
                    });
                    return Json(new { success = false, message = "Fotoğraf databaseden silinemedi." });
                }
                else
                {
                    return Json(new { success = true, message = "Fotoğraf silme işlemi başarılı." });
                }

            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    Culture = "tr",
                    ErrorLocation = "Admin Paneli DeleteAdditionalPhoto",
                    ErrorDetail = e.Message + " , " + e.InnerException + " " + "try catch hatası",
                    ErrorUrl = HttpContext.Request.Path
                });

                return Json(new { success = false, message = "Fotoğraf silinemedi. Hata oluştu." });
            }
        }

        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }
    }
}










