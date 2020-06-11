using AutoMapper;
using MailKit.Security;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MimeKit;
using System;
using System.Linq;

namespace MedikalMarket.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IMapper _mapper;

        public AccountController(ICustomerRepository customerRepo, IStringLocalizer<AccountController> localizer, IErrorLogRepository errorRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _localizer = localizer;
            _errorRepo = errorRepo;
            _mapper = mapper;
        }

        [Route("/tr/uye-girisi")]
        [Route("/en/login")]
        [Route("/ru/login")]
        public IActionResult Login()
        {
            LoginDto dto = new LoginDto();
            string userEmailCokie = Request.Cookies["userEmail"];
            ViewBag.RegAlert = HttpContext.Session.GetString("registerSuc") ?? "";
            ViewBag.warnTitle = _localizer["Başarılı"];
            HttpContext.Session.Remove("registerSuc");
            
            if (userEmailCokie!=null)
            {
                dto.RememberMe = true;
                dto.Email = userEmailCokie;
            }
            string newRegisterEmail = HttpContext.Session.GetString("newRegisterEmail");
            if (newRegisterEmail != null)
            {
                dto.Email = newRegisterEmail;
                HttpContext.Session.Remove("newRegisterEmail");
            }
            return View(dto);
        }

        [Route("/tr/yeni-uye-kaydi")]
        [Route("/en/register")]
        [Route("/ru/register")]
        public IActionResult Register()
        {
            return View(new RegisterDto());
        }

        [HttpPost]
        public JsonResult UpdateUserInfo(string email, string name, string password, bool sms, bool mail, string address, string phoneNumber)
        {
            if (!AuthCheck())
            {
                string msgTitle = _localizer["Uyarı"];
                string rspText = _localizer["Oturumunuz sonlandı. Lütfen tekrar giriş yapınız."];
                return Json(new { success = false, title = msgTitle, responseText = rspText, info="sessionExpire"});
            }

            var customerId = _customerRepo.GetEntityById(HttpContext.Session.GetObject<SessionDto>("customerInfo").Id).Id;

            var customerCount = _customerRepo.CountEntity(x => x.EmailAddress.Equals(email) && x.Id != customerId);
             
            if (customerCount >= 1)
            {
                string msgTitle = _localizer["Uyarı"];
                string rspText = _localizer["Girdiğiniz eposta başka bir kullanıcıya aittir."];
                return Json(new { success = false, title = msgTitle, responseText = rspText, info="emailCrash"});
            }
            else
            {
                var customer = _customerRepo.GetEntityById(HttpContext.Session.GetObject<SessionDto>("customerInfo").Id);
                customer.EmailAddress = email;
                customer.NameSurname = name;
                customer.Password = password;
                customer.IsSubscribedToSMS = sms;
                customer.IsSubscribedToEmail= mail;
                customer.Address = address;
                customer.CellPhoneNumber = phoneNumber;

                bool a = _customerRepo.UpdateEntity(customer);

                if (a)
                {
                    string msgTitle = _localizer["Başarılı"];
                    string rspText = _localizer["Bilgileriniz başarıyla güncellenmiştir."];
                    return Json(new { success = true, title = msgTitle, responseText = rspText, info = "updateSuccess" });
                }
                else
                {
                    string msgTitle = _localizer["Hata"];
                    string rspText = _localizer["Bilgi güncelleme işlemi başarısız. Hata oluştu. Lütfen bilgilerinizi kontrol ediniz."];
                    return Json(new { success = false, title = msgTitle, responseText = rspText, info = "updateFail" });
                }
            }
        }

        [HttpPost]
        public JsonResult Register(string email, string name, string password, bool sms, bool mail)
        {
            try
            {
                var customer = _customerRepo.AnyEntity(x => x.EmailAddress.Equals(email));

                if (customer)
                {
                    string msgTitle = _localizer["Uyarı"];
                    string rspText = _localizer["Girdiğiniz eposta sistemimizde kayıtlıdır."];
                    return Json(new { success = false, title = msgTitle, responseText = rspText });
                }
                else
                {
                    bool a= _customerRepo.CreateEntity(new Customer
                    {
                        NameSurname= name,
                        EmailAddress=email,
                        Password=password,
                        IsSubscribedToEmail=mail,
                        IsSubscribedToSMS=sms
                    });

                    if (a)
                    {
                        string msgTitle = _localizer["Başarılı"];
                        string rspText = _localizer["Kayıt işlemi başarılı...  Lütfen bilgilerinizi girerek oturum açınız."];
                        string url = _localizer["/tr/uye-girisi"];

                        HttpContext.Session.SetString("success", _localizer["Kayıt işlemi başarılı...  Lütfen bilgilerinizi girerek oturum açınız."]);

                        HttpContext.Session.SetString("newRegisterEmail", email);

                        return Json(new { success = true, title = msgTitle, responseText = rspText, togo= url });
                    }
                    else
                    {
                        string msgTitle = _localizer["Hata"];
                        string rspText = _localizer["Hata oluştu. Kayıt işlemi gerçekleştirilemedi. Lütfen tekrar deneyiniz."];

                        return Json(new { success = false, title = msgTitle, responseText = rspText });
                    }
                    
                }
            }
            catch (System.Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message,
                    ErrorLocation = "LoginCheck",
                    ErrorUrl = HttpContext.Request.Path
                });

                string msgTitle = _localizer["Hata"];
                string rspText = _localizer["Hata oluştu ve kayıt altına alındı."];

                return Json(new { success = false, title = msgTitle, responseText = rspText });
            }
        }

        public JsonResult SendMyPassword(string userEmail)
        {
            try
            {
                var user = _customerRepo.FindEntities(x => !x.IsDeleted && x.EmailAddress.Equals(userEmail)).SingleOrDefault();
                if (user == null)
                {
                    string rspText = _localizer["<div class='alert alert-danger' role='alert'>Girdiğiniz eposta adresi sistemimizde bulunamadı.</div>"];

                    return Json(new { success = false, responseText = rspText });
                }
                else
                {
                    MimeMessage message = new MimeMessage();
                    MailboxAddress from = new MailboxAddress("Premiummedikal.com",
                    "ubsisprojectmanagement@gmail.com");

                    message.From.Add(from);
                    MailboxAddress to = new MailboxAddress(_localizer["Premimummedikal.com Şifre Hatırlatma Mesajı"], user.EmailAddress);

                    message.To.Add(to);

                    message.Subject = _localizer["Premimummedikal.com Şifre Hatırlatma Mesajı"];

                    BodyBuilder bodyBuilder = new BodyBuilder();

                    string firstPart = _localizer["Sayın kullanıcımız, bu eposta şifre talebiniz nedeniyle gönderilmiştir. Premiumedikal.com sitesine giriş için kayıtlı şifreniz:"];
                    string secondPart = firstPart + " '" +  user.Password + "' ";
                    string lastPart = secondPart + " " + _localizer["Teşekkür ederiz."];

                    bodyBuilder.TextBody = lastPart;

                    message.Body = bodyBuilder.ToMessageBody();

                    MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();

                     
                        client.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);
                        client.Authenticate("ubsisprojectmanagement@gmail.com", "11791191");
                        client.Send(message);
                        client.Disconnect(true);
                        client.Dispose();

                    string rspText = _localizer["<div class='alert alert-success' role='alert'>Şifreniz eposta adresinize gönderilmiştir.</div>"];

                    return Json(new { success = false, responseText = rspText });
                }
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message,
                    ErrorLocation = "SendMyPassword",
                    ErrorUrl = HttpContext.Request.Path
                });

                string rspText = _localizer["<div class='alert alert-warning' role='alert'>Hata oluştu ve kayıt altına alındı.</div>"];
                return Json(new { success = false, responseText = rspText });
            }
            
        }

        [HttpPost]
        public JsonResult LoginCheck(string email, string password, bool rememberMe)
        {
            try
            {
                var customer = _customerRepo.FindEntities(x => x.EmailAddress.Equals(email) && x.Password.Equals(password)).FirstOrDefault();

                if (customer == null)
                {
                    string msgTitle = _localizer["Uyarı"];
                    string rspText = _localizer["Kullanıcı bulunamadı. Lütfen eposta ve şifrenizi kontrol ediniz..."];

                    return Json(new { success = false, title = msgTitle, responseText = rspText});
                }
                else
                {
                    if (rememberMe)
                    {
                        SetMyCookie("userEmail", customer.EmailAddress);
                    }
                    else
                    {
                        RemoveMyCookie("userEmail");
                        string userCok = Request.Cookies["userEmail"];
                    }

                    SessionDto sessionDto = new SessionDto();
                    sessionDto.Id = customer.Id;
                    sessionDto.NameSurname = customer.NameSurname;
                    HttpContext.Session.SetObject("customerInfo", sessionDto);

                    string msgTitle = _localizer["Başarılı"];
                    string rspText = _localizer["Giriş işlemi başarılı...  yönlendiriliyorsunuz..."];

                    return Json(new { success = true, title= msgTitle, responseText = rspText });
                }
            }
            catch (System.Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorDetail = e.Message,
                    ErrorLocation="LoginCheck",
                    ErrorUrl = HttpContext.Request.Path
                });

                string msgTitle = _localizer["Hata"];
                string rspText = _localizer["Hata oluştu ve kayıt altına alındı."];

                return Json(new { success = false, title = msgTitle, responseText = rspText });
            }
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

        [Route("/tr/cikis")]
        [Route("/en/logout")]
        [Route("/ru/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customerInfo");
            return LocalRedirect("/");
        }

        public IActionResult ValidateEmail(string email)
        {
            bool result = true;

            foreach (var item in _customerRepo.GetAllEntities().ToList())
            {
                if (item.EmailAddress == email)
                {
                    result = false;
                }
            }
            
            return Json(result);
        }

        [Route("/tr/hesabim")]
        [Route("/en/myaccount")]
        [Route("/ru/myaccount")]
        public IActionResult UserPanel()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var browserCult = locale.RequestCulture.UICulture.ToString();

            if (!AuthCheck())
            {
                return LocalRedirect("/");
            }
            int userId = HttpContext.Session.GetObject<SessionDto>("customerInfo").Id;
            var customerDto = _customerRepo.GetCustomerDtoWithFPs(userId, browserCult);
            return View(customerDto);
        }

        [NonAction]
        public bool AuthCheck()
        {
            return HttpContext.Session.GetInt32("customerInfo") == null ? false : true;
        }
    }
}
