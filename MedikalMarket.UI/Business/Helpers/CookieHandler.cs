using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Business.Helpers
{
    public class CookieHandler : ControllerBase
    {
        public void SetMyCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            Response.Cookies.Append(key, value);
        }

        public void RemoveMyCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        public string GetCulture()
        {
            return Request.Cookies[".AspNetCoreCulture"];
        }
    }
}
