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
    public class SliderComponent : ViewComponent
    {
        private readonly ISliderRepository _sliderRepo;
        private readonly IMapper _mapper;
        private readonly IErrorLogRepository _errorRepo;


        public SliderComponent(ISliderRepository sliderRepo, IMapper mapper, IErrorLogRepository errorRepo)
        {
            _sliderRepo = sliderRepo;
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
                ViewBag.culture = culture;

                var slidersDb = _sliderRepo.FindEntities(x => !x.IsDeleted && x.Culture.Equals(culture)).ToList();
                var sliderDtos = new List<SliderDto>();
                foreach (var slider in slidersDb)
                {
                    sliderDtos.Add(_mapper.Map<SliderDto>(slider));
                }
                return View(sliderDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "SliderComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
        }
    }
}
