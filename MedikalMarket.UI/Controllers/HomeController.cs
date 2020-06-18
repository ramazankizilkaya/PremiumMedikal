using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MedikalMarket.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IEmailNewsletterRepository _emailNewsletterRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IContactUsRepository _contactRepo;

        public HomeController(ILogger<HomeController> logger, ITopCategoryRepository topRepo, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IBrandRepository brandRepo, IProductRepository productRepo, IStringLocalizer<HomeController> localizer, IEmailNewsletterRepository emailNewsletterRepo, IErrorLogRepository errorRepo, IHostingEnvironment hostingEnvironment, IContactUsRepository contactRepo)
        {
            _logger = logger;
            _topRepo = topRepo;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _brandRepo = brandRepo;
            _productRepo = productRepo;
            _localizer = localizer;
            _emailNewsletterRepo = emailNewsletterRepo;
            _errorRepo = errorRepo;
            _hostingEnvironment = hostingEnvironment;
            _contactRepo = contactRepo;
        }
        //deneme
        [Route("/")]
        [Route("/home")]
        [Route("/anasayfa")]
        //[OutputCache(Duration = 120, VaryByCustom = HomeController.globalCulture]
        public IActionResult Index()
        {
            ViewBag.Alert = HttpContext.Session.GetString("alert");
            HttpContext.Session.Remove("alert");

            ViewBag.Success = HttpContext.Session.GetString("success");
            HttpContext.Session.Remove("success");

            //var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            //var browserCult = locale.RequestCulture.UICulture.ToString();
            //string culture = browserCult.Equals("tr") ? "tr" : browserCult.Equals("ru") ? "ru" : "en";
            //HttpContext.Session.SetString("culture", culture);

            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTime.Now.AddDays(1) }
            //    );

            return View();
        }

        public JsonResult SearchGivenInput(string term)
        {
            term = term.ToLower();
            try
            {
                if (!string.IsNullOrWhiteSpace(term) && !string.IsNullOrEmpty(term))
                {
                    var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                    var browserCult = locale.RequestCulture.UICulture.ToString();
                    List<string> resultList = new List<string>();

                    if (browserCult.Equals("tr"))
                    {
                        var proNames = _productRepo.FindEntities(x => !x.IsDeleted && x.NameTR.ToLower().Contains(term)).Take(10).Select(x => x.NameTR).ToList();
                        var topCateNames = _topRepo.FindEntities(x => !x.IsDeleted && x.NameTR.ToLower().Contains(term)).Take(10).Select(x => x.NameTR).ToList();
                        var midCateNames = _midRepo.FindEntities(x => !x.IsDeleted && x.NameTR.ToLower().Contains(term)).Take(10).Select(x => x.NameTR).ToList();
                        var subCateNames = _subRepo.FindEntities(x => !x.IsDeleted && x.NameTR.ToLower().Contains(term)).Take(10).Select(x => x.NameTR).ToList();
                        var brandNames = _brandRepo.FindEntities(x => !x.IsDeleted && x.BrandName.ToLower().Contains(term)).Take(10).Select(x => x.BrandName).ToList();

                        proNames.ForEach(x => resultList.Add(x));
                        topCateNames.ForEach(x => resultList.Add(x));
                        midCateNames.ForEach(x => resultList.Add(x));
                        subCateNames.ForEach(x => resultList.Add(x));
                        brandNames.ForEach(x => resultList.Add(x));

                        return Json(resultList.Take(10));
                    }
                    if (browserCult.Equals("ru"))
                    {
                        var proNames = _productRepo.FindEntities(x => !x.IsDeleted && x.NameRU.ToLower().Contains(term)).Take(10).Select(x => x.NameRU).ToList();
                        var topCateNames = _topRepo.FindEntities(x => !x.IsDeleted && x.NameRU.ToLower().Contains(term)).Take(10).Select(x => x.NameRU).ToList();
                        var midCateNames = _midRepo.FindEntities(x => !x.IsDeleted && x.NameRU.ToLower().Contains(term)).Take(10).Select(x => x.NameRU).ToList();
                        var subCateNames = _subRepo.FindEntities(x => !x.IsDeleted && x.NameRU.ToLower().Contains(term)).Take(10).Select(x => x.NameRU).ToList();
                        var brandNames = _brandRepo.FindEntities(x => !x.IsDeleted && x.BrandName.ToLower().Contains(term)).Take(10).Select(x => x.BrandName).ToList();

                        proNames.ForEach(x => resultList.Add(x));
                        topCateNames.ForEach(x => resultList.Add(x));
                        midCateNames.ForEach(x => resultList.Add(x));
                        subCateNames.ForEach(x => resultList.Add(x));
                        brandNames.ForEach(x => resultList.Add(x));

                        return Json(resultList.Take(10));
                    }
                    else
                    {
                        var proNames = _productRepo.FindEntities(x => !x.IsDeleted && x.NameEN.ToLower().Contains(term)).Take(10).Select(x => x.NameEN).ToList();
                        var topCateNames = _topRepo.FindEntities(x => !x.IsDeleted && x.NameEN.ToLower().Contains(term)).Take(10).Select(x => x.NameEN).ToList();
                        var midCateNames = _midRepo.FindEntities(x => !x.IsDeleted && x.NameEN.ToLower().Contains(term)).Take(10).Select(x => x.NameEN).ToList();
                        var subCateNames = _subRepo.FindEntities(x => !x.IsDeleted && x.NameEN.ToLower().Contains(term)).Take(10).Select(x => x.NameEN).ToList();
                        var brandNames = _brandRepo.FindEntities(x => !x.IsDeleted && x.BrandName.ToLower().Contains(term)).Take(10).Select(x => x.BrandName).ToList();

                        proNames.ForEach(x => resultList.Add(x));
                        topCateNames.ForEach(x => resultList.Add(x));
                        midCateNames.ForEach(x => resultList.Add(x));
                        subCateNames.ForEach(x => resultList.Add(x));
                        brandNames.ForEach(x => resultList.Add(x));

                        return Json(resultList.Take(10));
                    }
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Deneme()
        {
            return View();
        }

        [Route("/tr/hakkimizda")]
        [Route("/en/about-us")]
        [Route("/ru/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpPost]
        //[NonAction]
        public JsonResult AddToNewsletter(string email)
        {
            if (email == null)
            {
                return Json(new { success = false, responseText = _localizer["Lütfen geçerli bir eposta adresi giriniz."].ToString() });
            }

            var emailDbcount = _emailNewsletterRepo.AnyEntity(x => x.EmailAddress.Equals(email));

            if (emailDbcount)
            {
                return Json(new { success = false, responseText = _localizer["Eposta aboneliğiniz mevcuttur."].ToString() });
            }

            else
            {
                #region IP Kontrolü
                string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    remoteIpAddress = Request.Headers["X-Forwarded-For"];
                }
                #endregion

                EmailNewsletter newsLetter = new EmailNewsletter
                {
                    EmailAddress = email,
                    UserIp = remoteIpAddress
                };

                _emailNewsletterRepo.CreateEntity(newsLetter);

                return Json(new { success = true, responseText = _localizer["E-posta adresiniz bültenimize kaydedilmiştir. Teşekkür ederiz."].ToString() });
            }
        }

        [Route("/tr/iletisim")]
        [Route("/en/contact-us")]
        [Route("/ru/contact-us")]
        public IActionResult ContactUs()
        {
            return View(new ContactUsDto());
        }

        [HttpPost]
        public JsonResult ChangeSecurityImage()
        {
            string folderPath2 = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "SecurityImages");

            List<string> fileNames = new List<string>();

            if (Directory.Exists(folderPath2))
            {
                foreach (string file in Directory.EnumerateFiles(folderPath2))
                {
                    string result = Path.GetFileName(file);
                    fileNames.Add(result);
                }
            }

            fileNames.ShuffleMyList();
            return Json(new { success = true, fileName = fileNames[0] });
        }

        [HttpPost]
        public JsonResult SendContactUsMessage(string nameSurname, string email, string subject, string message)
        {
            bool a = _contactRepo.CreateEntity(new ContactUs
            {
                NameSurname = nameSurname,
                EmailAddress = email,
                Subject = subject,
                Message = message
            });
            if (a)
            {
                HttpContext.Session.SetString("success", _localizer["Mesajınız alınmıştır. En kısa sürede dönüş yapılacaktır. Teşekkür ederiz."]);
                return Json(new { success = true });
            }
            else
            {
                string msgTitle = _localizer["Hata"];
                string rspText = _localizer["Hata oluştu. Mesaj gönderme işlemi gerçekleştirilemedi. Lütfen tekrar deneyiniz."];
                return Json(new { success = false, title = msgTitle, responseText = rspText });
            }
        }


        [HttpPost]
        public JsonResult DecodeText(string imageFileName)
        {
            string myText = imageFileName.DecodeText();
            return Json(new { success = true, result = myText });
        }

        [HttpPost]
        public JsonResult FindSearchItem(string searchText)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var browserCult = locale.RequestCulture.UICulture.ToString();

            try
            {
                if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                {
                    return Json(new { success = false });
                }
                if (browserCult.Equals("tr"))
                {
                    var product = _productRepo.GetProductWithNameTR(searchText);
                    if (product != null)
                    {
                        string myLink = $"/tr/urun-detay/{product.Brand.BandNameUrl}/{product.TopCategory?.TopCategoryNameUrlTR}/{product.MiddleCategory?.MiddleCategoryNameUrlTR}/{product.SubCategory?.SubCategoryNameUrlTR}/{product.ProductNameUrlTR}";

                        myLink = myLink.Replace("///", "/", StringComparison.Ordinal);
                        myLink = myLink.Replace("//", "/", StringComparison.Ordinal);

                        return Json(new { success = true, result = myLink });
                    }

                    var topCate = _topRepo.GetTopCateByNameTR(searchText);
                    if (topCate != null)
                    {
                        string myLink = $"/tr/kategoriler/{topCate.TopCategoryNameUrlTR}/sayfa/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var midCate = _midRepo.GetMidCateByNameTR(searchText);
                    if (midCate != null)
                    {
                        string myLink = $"/tr/kategoriler/{midCate.TopCategory.TopCategoryNameUrlTR}/{midCate.MiddleCategoryNameUrlTR}/sayfa/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var subCate = _subRepo.GetSubCateByNameTR(searchText);
                    if (subCate != null)
                    {
                        string myLink = $"/tr/kategoriler/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlTR}/{subCate.MiddleCategory.MiddleCategoryNameUrlTR}/{subCate.SubCategoryNameUrlTR}/sayfa/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var brand = _brandRepo.GetBrandByName(searchText);
                    if (brand != null)
                    {
                        string myLink = $"/tr/markalar/{brand.BandNameUrl}/sayfa/1";
                        return Json(new { success = true, result = myLink });
                    }
                }
                if (browserCult.Equals("ru"))
                {
                    var product = _productRepo.GetProductWithNameRU(searchText);
                    if (product != null)
                    {
                        string myLink = $"/ru/product-detail/{product.Brand.BandNameUrl}/{product.TopCategory?.TopCategoryNameUrlEN}/{product.MiddleCategory?.MiddleCategoryNameUrlEN}/{product.SubCategory?.SubCategoryNameUrlEN}/{product.ProductNameUrlEN}";

                        myLink = myLink.Replace("///", "/", StringComparison.Ordinal);
                        myLink = myLink.Replace("//", "/", StringComparison.Ordinal);

                        return Json(new { success = true, result = myLink });
                    }

                    var topCate = _topRepo.GetTopCateByNameRU(searchText);
                    if (topCate != null)
                    {
                        string myLink = $"/ru/categories/{topCate.TopCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var midCate = _midRepo.GetMidCateByNameRU(searchText);
                    if (midCate != null)
                    {
                        string myLink = $"/ru/categories/{midCate.TopCategory.TopCategoryNameUrlEN}/{midCate.MiddleCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var subCate = _subRepo.GetSubCateByNameRU(searchText);
                    if (subCate != null)
                    {
                        string myLink = $"/ru/categories/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlEN}/{subCate.MiddleCategory.MiddleCategoryNameUrlEN}/{subCate.SubCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var brand = _brandRepo.GetBrandByName(searchText);
                    if (brand != null)
                    {
                        string myLink = $"/ru/brands/{brand.BandNameUrl}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                }
                else
                {
                    var product = _productRepo.GetProductWithNameEN(searchText);
                    if (product != null)
                    {
                        string myLink = $"/en/product-detail/{product.Brand.BandNameUrl}/{product.TopCategory?.TopCategoryNameUrlEN}/{product.MiddleCategory?.MiddleCategoryNameUrlEN}/{product.SubCategory?.SubCategoryNameUrlEN}/{product.ProductNameUrlEN}";

                        myLink = myLink.Replace("///", "/", StringComparison.Ordinal);
                        myLink = myLink.Replace("//", "/", StringComparison.Ordinal);

                        return Json(new { success = true, result = myLink });
                    }

                    var topCate = _topRepo.GetTopCateByNameEN(searchText);
                    if (topCate != null)
                    {
                        string myLink = $"/en/categories/{topCate.TopCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var midCate = _midRepo.GetMidCateByNameEN(searchText);
                    if (midCate != null)
                    {
                        string myLink = $"/en/categories/{midCate.TopCategory.TopCategoryNameUrlEN}/{midCate.MiddleCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var subCate = _subRepo.GetSubCateByNameEN(searchText);
                    if (subCate != null)
                    {
                        string myLink = $"/en/categories/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlEN}/{subCate.MiddleCategory.MiddleCategoryNameUrlEN}/{subCate.SubCategoryNameUrlEN}/page/1";
                        return Json(new { success = true, result = myLink });
                    }

                    var brand = _brandRepo.GetBrandByName(searchText);
                    if (brand != null)
                    {
                        string myLink = $"/en/brands/{brand.BandNameUrl}/page/1";
                        return Json(new { success = true, result = myLink });
                    }
                }
                string mes = browserCult.Equals("tr") ? "Sonuç bulunamadı" : browserCult.Equals("ru") ? "Не было результатов" : "There were no results";
                return Json(new { success = false, message = mes });

            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "Home Controller FindSearchItem",
                    Culture = browserCult,
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });

                string mes = browserCult.Equals("tr") ? "Sonuç bulunamadı" : browserCult.Equals("ru") ? "Не было результатов" : "There were no results";

                return Json(new { success = false, message = mes });
            }
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTime.Now.AddDays(1) }
                );

                HttpContext.Session.SetString("culture", culture);

                string[] urlArr = returnUrl.Split("/");

                //anasayfadan geliyorsa
                if (urlArr.Length == 2 && urlArr[0].Equals("") && urlArr[1].Equals(""))
                {
                    return LocalRedirect("/");
                }

                //hakkımızda sayfasından geliyorsa
                if (urlArr.Length == 3 && (urlArr[2].Equals("hakkimizda") || urlArr[2].Equals("about-us")))
                {
                    if (culture.Equals("tr"))
                    {
                        return LocalRedirect("/tr/hakkimizda");
                    }
                    if (culture.Equals("ru"))
                    {
                        return LocalRedirect("/ru/about-us");
                    }
                    else
                    {
                        return LocalRedirect("/en/about-us");
                    }
                }

                //iletişim sayfasından geliyorsa
                if (urlArr.Length == 3 && (urlArr[2].Equals("iletisim") || urlArr[2].Equals("contact-us")))
                {
                    if (culture.Equals("tr"))
                    {
                        return LocalRedirect("/tr/iletisim");
                    }
                    if (culture.Equals("ru"))
                    {
                        return LocalRedirect("/ru/contact-us");
                    }
                    else
                    {
                        return LocalRedirect("/en/contact-us");
                    }
                }

                //login sayfasından geliyorsa
                if (urlArr.Length == 3 && (urlArr[2].Equals("uye-girisi") || urlArr[2].Equals("login")))
                {
                    if (culture.Equals("tr"))
                    {
                        return LocalRedirect("/tr/uye-girisi");
                    }
                    if (culture.Equals("ru"))
                    {
                        return LocalRedirect("/ru/login");
                    }
                    else
                    {
                        return LocalRedirect("/en/login");
                    }
                }

                //Register sayfasından geliyorsa
                if (urlArr.Length == 3 && (urlArr[2].Equals("yeni-uye-kaydi") || urlArr[2].Equals("register")))
                {
                    if (culture.Equals("tr"))
                    {
                        return LocalRedirect("/tr/yeni-uye-kaydi");
                    }
                    if (culture.Equals("ru"))
                    {
                        return LocalRedirect("/ru/register");
                    }
                    else
                    {
                        return LocalRedirect("/en/register");
                    }
                }

                //Userpanel sayfasından geliyorsa
                if (urlArr.Length == 3 && (urlArr[2].Equals("myaccount") || urlArr[2].Equals("hesabim")))
                {
                    if (culture.Equals("tr"))
                    {
                        return LocalRedirect("/tr/hesabim");
                    }
                    if (culture.Equals("ru"))
                    {
                        return LocalRedirect("/ru/myaccount");
                    }
                    else
                    {
                        return LocalRedirect("/en/myaccount");
                    }
                }

                //marka sayfasından geliyorsa
                if (urlArr.Length == 6 && (urlArr[2].Equals("brands") || urlArr[2].Equals("markalar")))
                {
                    if (culture.Equals("tr"))
                    {
                        string direction = $"/tr/markalar/{urlArr[3]}/sayfa/{urlArr[5]}";
                        return LocalRedirect(direction);
                    }
                    if (culture.Equals("ru"))
                    {
                        string direction = $"/ru/brands/{urlArr[3]}/page/{urlArr[5]}";
                        return LocalRedirect(direction);
                    }
                    else
                    {
                        string direction = $"/en/brands/{urlArr[3]}/page/{urlArr[5]}";
                        return LocalRedirect(direction);
                    }
                }

                //array lengthi 6 ise topcateden gelmiş demektir.
                if (urlArr.Length == 6 && (urlArr[2].Equals("categories") || urlArr[2].Equals("kategoriler")))
                {
                    if (urlArr[1].Equals(culture))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        int topId = 0;

                        if (urlArr[1].Equals("tr"))
                        {
                            var topCateTR = _topRepo.FindEntities(x => x.TopCategoryNameUrlTR.Equals(urlArr[3])).SingleOrDefault();
                            topId = topCateTR?.Id ?? 0;
                        }
                        else
                        {
                            var topCateEN = _topRepo.FindEntities(x => x.TopCategoryNameUrlEN.Equals(urlArr[3])).SingleOrDefault();
                            topId = topCateEN?.Id ?? 0;
                        }
                        var finalTopCate = _topRepo.GetEntityById(topId);

                        string topCate = culture.Equals("tr") ? finalTopCate.TopCategoryNameUrlTR : finalTopCate.TopCategoryNameUrlEN;
                        string pageText = culture.Equals("tr") ? "sayfa" : "page";
                        string cates = culture.Equals("tr") ? "kategoriler" : "categories";

                        string newReturnUrl = $"/{culture}/{cates}/{topCate}/{pageText}/{urlArr[5]}";
                        return LocalRedirect(newReturnUrl);
                    }
                }

                //array lengthi 7 ise midcateden gelmiş demektir.
                if (urlArr.Length == 7 && (urlArr[2].Equals("categories") || urlArr[2].Equals("kategoriler")))
                {
                    if (urlArr[1].Equals(culture))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        int midId = 0;

                        if (urlArr[1].Equals("tr"))
                        {
                            var topCateTR = _topRepo.FindEntities(x => x.TopCategoryNameUrlTR.Equals(urlArr[3])).SingleOrDefault();
                            var midCateTR = _midRepo.FindEntities(x => x.MiddleCategoryNameUrlTR.Equals(urlArr[4]) && x.TopCategoryId == topCateTR.Id).SingleOrDefault();
                            midId = midCateTR?.Id ?? 0;
                        }
                        else
                        {
                            var topCateEN = _topRepo.FindEntities(x => x.TopCategoryNameUrlEN.Equals(urlArr[3])).SingleOrDefault();
                            var midCateEN = _midRepo.FindEntities(x => x.MiddleCategoryNameUrlEN.Equals(urlArr[4]) && x.TopCategoryId == topCateEN.Id).SingleOrDefault();
                            midId = midCateEN?.Id ?? 0;
                        }
                        var finalMidCate = _midRepo.GetEntityById(midId);
                        var finalTopCate = _topRepo.GetEntityById(finalMidCate.TopCategoryId);

                        string topCate = culture.Equals("tr") ? finalTopCate.TopCategoryNameUrlTR : finalTopCate.TopCategoryNameUrlEN;
                        string midCate = culture.Equals("tr") ? finalMidCate.MiddleCategoryNameUrlTR : finalMidCate.MiddleCategoryNameUrlEN;
                        string pageText = culture.Equals("tr") ? "sayfa" : "page";
                        string cates = culture.Equals("tr") ? "kategoriler" : "categories";

                        string newReturnUrl = $"/{culture}/{cates}/{topCate}/{midCate}/{pageText}/{urlArr[6]}";
                        return LocalRedirect(newReturnUrl);
                    }
                }

                //array lengthi 8 ise subcateden gelmiş demektir.
                if (urlArr.Length == 8 && (urlArr[2].Equals("categories") || urlArr[2].Equals("kategoriler")))
                {
                    if (urlArr[1].Equals(culture))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        int subId = 0;

                        if (urlArr[1].Equals("tr"))
                        {
                            var topCateTR = _topRepo.FindEntities(x => x.TopCategoryNameUrlTR.Equals(urlArr[3])).SingleOrDefault();

                            var midCateTR = _midRepo.FindEntities(x => x.MiddleCategoryNameUrlTR.Equals(urlArr[4]) && x.TopCategoryId == topCateTR.Id).SingleOrDefault();

                            var subCateTR = _subRepo.FindEntities(x => x.SubCategoryNameUrlTR.Equals(urlArr[5]) && x.MiddleCategoryId == midCateTR.Id).SingleOrDefault();

                            subId = subCateTR?.Id ?? 0;
                        }
                        else
                        {
                            var topCateEN = _topRepo.FindEntities(x => x.TopCategoryNameUrlEN.Equals(urlArr[3])).SingleOrDefault();

                            var midCateEN = _midRepo.FindEntities(x => x.MiddleCategoryNameUrlEN.Equals(urlArr[4]) && x.TopCategoryId == topCateEN.Id).SingleOrDefault();

                            var subCateEN = _subRepo.FindEntities(x => x.SubCategoryNameUrlEN.Equals(urlArr[5]) && x.MiddleCategoryId == midCateEN.Id).SingleOrDefault();
                            subId = subCateEN?.Id ?? 0;
                        }
                        var finalSubCate = _subRepo.GetEntityById(subId);
                        var finalMidCate = _midRepo.GetEntityById(finalSubCate.MiddleCategoryId);
                        var finalTopCate = _topRepo.GetEntityById(finalMidCate.TopCategoryId);

                        string topCate = culture.Equals("tr") ? finalTopCate.TopCategoryNameUrlTR : finalTopCate.TopCategoryNameUrlEN;
                        string midCate = culture.Equals("tr") ? finalMidCate.MiddleCategoryNameUrlTR : finalMidCate.MiddleCategoryNameUrlEN;
                        string subCate = culture.Equals("tr") ? finalSubCate.SubCategoryNameUrlTR : finalSubCate.SubCategoryNameUrlEN;
                        string pageText = culture.Equals("tr") ? "sayfa" : "page";
                        string cates = culture.Equals("tr") ? "kategoriler" : "categories";

                        string newReturnUrl = $"/{culture}/{cates}/{topCate}/{midCate}/{subCate}/{pageText}/{urlArr[7]}";
                        return LocalRedirect(newReturnUrl);
                    }
                }

                //array lengthi 5-6-7 ise single producttan gelmiş demektir.
                if ((urlArr.Length == 6 || urlArr.Length == 7 || urlArr.Length == 8) && (urlArr[2].Equals("urun-detay") || urlArr[2].Equals("product-detail")))
                {
                    if (urlArr[1].Equals(culture))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        int proId = 0;

                        if (urlArr[1].Equals("tr"))
                        {
                            var productTR = _productRepo.FindEntities(x => x.ProductNameUrlTR.Equals(urlArr[urlArr.Length - 1])).SingleOrDefault();
                            proId = productTR?.Id ?? 0;
                        }
                        else
                        {
                            var productEN = _productRepo.FindEntities(x => x.ProductNameUrlEN.Equals(urlArr[urlArr.Length - 1])).SingleOrDefault();
                            proId = productEN?.Id ?? 0;
                        }
                        var finalProduct = _productRepo.GetProductWithId(proId);

                        string topCate = null;
                        string midCate = null;
                        string subCate = null;
                        string product = null;
                        string brand = urlArr[3];
                        string cates = culture.Equals("tr") ? "urun-detay" : "product-detail";

                        var topCategory = _topRepo.GetEntityById(finalProduct.TopCategoryId);
                        topCate = culture.Equals("tr") ? topCategory.TopCategoryNameUrlTR : topCategory.TopCategoryNameUrlEN;
                        product = culture.Equals("tr") ? finalProduct.ProductNameUrlTR : finalProduct.ProductNameUrlEN;
                        //mid ve sub boşsa buna gider
                        if (urlArr.Length == 6)
                        {
                            return LocalRedirect($"/{culture}/{cates}/{brand}/{topCate}/{product}");
                        }
                        //mid ve sub boş değilse buraya düşer o yüzden mide bakabilirim
                        var midCategory = _midRepo.GetEntityById((int)finalProduct.MiddleCategoryId);
                        midCate = culture.Equals("tr") ? midCategory.MiddleCategoryNameUrlTR : midCategory.MiddleCategoryNameUrlEN;

                        //sub boşsa buna gider
                        if (urlArr.Length == 7)
                        {
                            return LocalRedirect($"/{culture}/{cates}/{brand}/{topCate}/{midCate}/{product}");
                        }

                        //sub boş değilse buraya düşer o yüzden suba bakabilirim
                        var subCategory = _subRepo.GetEntityById((int)finalProduct.SubCategoryId);
                        subCate = culture.Equals("tr") ? subCategory.SubCategoryNameUrlTR : subCategory.SubCategoryNameUrlEN;

                        //sub doluysa buna gider
                        if (urlArr.Length == 8)
                        {
                            return LocalRedirect($"/{culture}/{cates}/{brand}/{topCate}/{midCate}/{subCate}/{product}");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "SetLanguage",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                return LocalRedirect("/");
            }
            return LocalRedirect("/");
        }
    }
}
