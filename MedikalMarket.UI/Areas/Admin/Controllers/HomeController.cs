using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedikalMarket.UI.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
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
        private readonly IAdminRepository _adminRepo;
        private readonly IAdProductRepository _adproRepo;
        private readonly IContactUsRepository _contactRepo;
        private readonly IEmailNewsletterRepository _newsRepo;
        private readonly IFavoriteProductRepository _favRepo;
        private readonly IMiniSliderRepository _miniRepo;
        private readonly ISliderRepository _sliderRepo;

        public HomeController(ITopCategoryRepository topRepo, IProductRepository productRepo, IErrorLogRepository errorRepo, IHttpContextAccessor httpContextAccessor, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IFavoriteProductRepository fpRepo, ICustomerRepository customerRepo, IBrandRepository brandRepo, IMapper mapper, IWebHostEnvironment environment, IProductPhotoRepository photoRepo, IAdminRepository adminRepo, IAdProductRepository adproRepo, IContactUsRepository contactRepo, IEmailNewsletterRepository newsRepo, IFavoriteProductRepository favRepo, IMiniSliderRepository miniRepo, ISliderRepository sliderRepo)
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
            _adminRepo = adminRepo;
            _adproRepo = adproRepo;
            _contactRepo = contactRepo;
            _newsRepo = newsRepo;
            _favRepo = favRepo;
            _miniRepo = miniRepo;
            _sliderRepo = sliderRepo;
        }

        //[Route("/Admin")]
        //[Route("/Admin/Home/Index")]
        public IActionResult Index()
        {
            if (!AuthCheck())
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            Dictionary<string, int> infoList = new Dictionary<string, int>
            {
                { "Toplam Ürün Sayısı: ", _productRepo.GetTotalProductCount() },
                { "Akif Ürün Sayısı: ", _productRepo.CountEntity(x => !x.IsDeleted) },
                { "Pasif Ürün Sayısı: ", _productRepo.CountEntity(x => !!x.IsDeleted) },
                { "Sinilmiş Ürün Sayısı: ", _productRepo.CountEntity(x => x.IsDeleted) },
                { "Aktif Ana Kategori Sayısı: ", _topRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Aktif Orta Kategori Sayısı: ", _midRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Aktif Alt Kategori Sayısı: ", _subRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Pasif Ana Kategori Sayısı: ", _topRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Pasif Orta Kategori Sayısı: ", _midRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Pasif Alt Kategori Sayısı: ", _subRepo.FindEntities(x => !x.IsDeleted).Count() },
                { "Admin Sayısı: ", _adminRepo.GetAllEntities().Count() },
                { "Marka Sayısı: ", _brandRepo.GetAllEntities().Count() },
                { "Siteye Gönderilen İletişim Mesajı Sayısı: ", _contactRepo.GetAllEntities().Count() },
                { "Kayıtlı Müşteri Sayısı: ", _customerRepo.GetAllEntities().Count() },
                { "Eposta Aboneliği Sayısı: ", _newsRepo.GetAllEntities().Count() },
                { "Hata Log Sayısı: ", _errorRepo.GetAllEntities().Count() },
                { "Favoriye Eklenen Ürün Sayısı Sayısı: ", _favRepo.GetAllEntities().Count() },
                { "Toplam Slider Sayısı: ", _sliderRepo.GetAllEntities().Count() },
                { "Türkçe Slider Sayısı: ", _sliderRepo.FindEntities(x => x.Culture.Equals("tr")).Count() },
                { "İngilizce Slider Sayısı: ", _sliderRepo.FindEntities(x => x.Culture.Equals("en")).Count() },
                { "Rusça Mini Sayısı: ", _sliderRepo.FindEntities(x => x.Culture.Equals("ru")).Count() },

                { "Toplam Reklam Sayısı: ", _adproRepo.GetAllEntities().Count() },
                { "Türkçe Reklam Sayısı: ", _adproRepo.FindEntities(x => x.Culture.Equals("tr")).Count() },
                { "İngilizce Reklam Sayısı: ", _adproRepo.FindEntities(x => x.Culture.Equals("en")).Count() },
                { "Rusça Reklam Sayısı: ", _adproRepo.FindEntities(x => x.Culture.Equals("ru")).Count() },

                { "Toplam Mini Slider Sayısı: ", _miniRepo.GetAllEntities().Count() },
                { "Türkçe Mini Slider Sayısı: ", _miniRepo.FindEntities(x => x.Culture.Equals("tr")).Count() },
                { "İngilizce Mini Slider Sayısı: ", _miniRepo.FindEntities(x => x.Culture.Equals("en")).Count() },
                { "Rusça Mini Slider Sayısı: ", _miniRepo.FindEntities(x => x.Culture.Equals("ru")).Count() }
            };

            return View(infoList);
        }


        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetString("adminInfo") == null ? false : true;
        }
    }
}