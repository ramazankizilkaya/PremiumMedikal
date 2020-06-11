using AutoMapper;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Components
{
    public class MiniSliderComponent : ViewComponent
    {
        private readonly IMiniSliderRepository _miniSliderRepo;
        private readonly IMapper _mapper;
        private readonly IErrorLogRepository _errorRepo;

        public MiniSliderComponent(IMiniSliderRepository miniSliderRepo, IMapper mapper, IErrorLogRepository errorRepo)
        {
            _miniSliderRepo = miniSliderRepo;
            _mapper = mapper;
            _errorRepo = errorRepo;
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
                var miniSlidersDb = _miniSliderRepo.FindEntities(x => !x.IsDeleted && x.Culture.Equals(culture));
                var miniSliderDtos = new List<MiniSliderDto>();
                ViewBag.culture = culture;
                foreach (var miniSlider in miniSlidersDb)
                {
                    miniSliderDtos.Add(_mapper.Map<MiniSliderDto>(miniSlider));
                }
                return View(miniSliderDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "MiniSliderComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture = culture
                });
                return null;
            }
        }
    }
}
