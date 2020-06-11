using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedikalMarket.UI.Areas.Admin.Dtos;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MedikalMarket.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IErrorLogRepository _errorRepo;

        public AccountController(IAdminRepository adminRepo, IErrorLogRepository errorRepo)
        {
            _adminRepo = adminRepo;
            _errorRepo = errorRepo;
        }

        public IActionResult Login()
        {
            string adminMailCookie = Request.Cookies["adminEmail"];
            ViewBag.adminEmailCokie = adminMailCookie;
            ViewBag.checkTest = adminMailCookie != null;
            return View();
        }

        [HttpPost]
        public JsonResult LoginCheck(string email, string password, bool rember)
        {
            try
            {
                var admin = _adminRepo.FindEntities(x => x.EmailAddress.Equals(email) && x.Password.Equals(password)).FirstOrDefault();

                if (admin == null)
                {
                    string msgTitle = "Uyarı";
                    string rspText = "Kullanıcı bulunamadı. Lütfen eposta ve şifrenizi kontrol ediniz...";
                    return Json(new { success = false, title = msgTitle, responseText = rspText });
                }
                else
                {
                    if (rember)
                    {
                        SetMyCookie("adminEmail", email);
                    }
                    else
                    {
                        RemoveMyCookie("adminEmail");
                    }

                    HttpContext.Session.SetString("adminInfo", email);
                    string msgTitle = "Başarılı";
                    string rspText = "Giriş işlemi başarılı...  yönlendiriliyorsunuz...";
                    return Json(new { success = true, title = msgTitle, responseText = rspText });
                }
            }
            catch (System.Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message,
                    ErrorLocation = "AccountController AdminLoginCheck",
                    ErrorUrl = HttpContext.Request.Path
                });

                string msgTitle ="Hata";
                string rspText = "Hata oluştu ve kayıt altına alındı.";

                return Json(new { success = false, title = msgTitle, responseText = rspText });
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("adminInfo");
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        public void SetMyCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            Response.Cookies.Append(key, value);
        }

        public void RemoveMyCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

    }
}
