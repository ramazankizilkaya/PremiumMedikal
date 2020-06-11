using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedikalMarket.UI.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            if (HttpContext.Session.GetString("userCulture") == null)
            {
                HttpContext.Session.SetString("userCulture", culture);
            }

            if (HttpContext.Session.GetString("userCulture")?.Equals(culture) == false)
            {
                HttpContext.Session.SetString("userCulture", culture);
            }
        }
    }
}