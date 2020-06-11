using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MedikalMarket.UI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IStringLocalizer<ErrorController> _localizer;


        public ErrorController(IStringLocalizer<ErrorController> localizer)
        {
            _localizer = localizer;
        }

        [Route("Error/500")]
        public IActionResult Error500()
        {
            ViewBag.ErrorMessage = _localizer["Beklenmedik bir hata oluştu. Hata kayıt altına alınmıştır. Sabrınız için teşekkürler..."];
            ViewBag.Home = _localizer["Anasayfa"];
            ViewBag.Title = _localizer["Hata"];
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                ViewBag.ErrorMessage2 = exceptionFeature.Error.Message;
                ViewBag.RouteOfException = exceptionFeature.Path;
            }
            return View();
        }


        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewBag.ErrorCode = statusCode;
            ViewBag.Home = _localizer["Anasayfa"];
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = _localizer["Aradığınız sayfa bulunamadı. Hata kayıt altına alınmıştır. Sabrınız için teşekkürler..."];
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 500:
                    ViewBag.ErrorMessage = _localizer["Beklenmedik bir hata oluştu. Hata kayıt altına alınmıştır. Sabrınız için teşekkürler..."];
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
            }
            return View();
        }
    }
}