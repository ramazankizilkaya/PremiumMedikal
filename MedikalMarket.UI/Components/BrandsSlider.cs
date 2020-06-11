using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedikalMarket.UI.Components
{
    public class BrandsSlider : ViewComponent
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IErrorLogRepository _errorRepo;

        public BrandsSlider(IBrandRepository brandRepo, IErrorLogRepository errorRepo)
        {
            _brandRepo = brandRepo;
            _errorRepo = errorRepo;
        }

        public IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                var brands = _brandRepo.GetAllBrandsWithFullJoin().Where(x => !x.IsDeleted).ToList();
                var brandDtos = new List<BrandsSliderDto>();
                foreach (var item in brands/*.Where(x=>x.Products.Count > 0)*/)
                {
                    BrandsSliderDto dto = new BrandsSliderDto();
                    dto.BrandName = item.BrandName;
                    dto.PhotoFileName = item.PhotoFileName;
                    dto.BrandNameUrl = item.BrandName.ConvertToFriendlyUrl();

                    switch (culture)
                    {
                        case "tr":
                            dto.PhotoAltTag = item.PhotoAltTagTR;
                            ViewBag.culture = "tr";
                            break;
                        case "en":
                            dto.PhotoAltTag = item.PhotoAltTagEN;
                            ViewBag.culture = "en";
                            break;
                        case "ru":
                            dto.PhotoAltTag = item.PhotoAltTagRU;
                            ViewBag.culture = "ru";
                            break;
                        default:
                            dto.PhotoAltTag = item.PhotoAltTagEN;
                            ViewBag.culture = "en";
                            break;
                    }
                    brandDtos.Add(dto);
                }
                return View(brandDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "BrandsSliderComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture = culture
                });
                return null;
            }
        }
    }
}
