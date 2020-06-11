using AutoMapper;
using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Components
{
    public class AdsProductComponent : ViewComponent
    {
        private readonly IAdProductRepository _adproRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AdsProductComponent> _localizer;
        private readonly IHttpContextAccessor _accesor;

        public AdsProductComponent(IAdProductRepository adproRepo, IMapper mapper, IErrorLogRepository errorRepo, IStringLocalizer<AdsProductComponent> localizer, IHttpContextAccessor accesor)
        {
            _adproRepo = adproRepo;
            _mapper = mapper;
            _errorRepo = errorRepo;
            _localizer = localizer;
            _accesor = accesor;
        }

        public IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();
            try
            {
                if (culture != "tr" && culture != "ru")
                {
                    culture = "en";
                }
                ViewBag.culture = culture;

                var adprosDb = _adproRepo.FindEntities(x => !x.IsDeleted && x.Culture.Equals(culture)).ToList();
                adprosDb.ShuffleMyList();
                var adproDtos = new List<AdProductDto>();
                foreach (var adpro in adprosDb.Take(6))
                {
                    adproDtos.Add(_mapper.Map<AdProductDto>(adpro));
                }
                return View(adproDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "AdsProductComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture= culture
                });

                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                //_accesor.HttpContext.Response.Redirect("/");
                return null;
            }
        }
    }
}
