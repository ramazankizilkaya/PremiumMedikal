using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedikalMarket.UI.Components
{
    public class FooterTopCategories : ViewComponent
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IErrorLogRepository _errorRepo;


        public FooterTopCategories(ITopCategoryRepository topRepo, IErrorLogRepository errorRepo)
        {
            _topRepo = topRepo;
            _errorRepo = errorRepo;
        }

        public IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                //var topcatesRU = _topRepo.FindEntities(x => !x.IsDeleted).Select(x => x.NameRU);
                var topcateDtos = new List<FooterTopCategoryDto>();

                switch (culture)
                {
                    case "tr":
                        ViewBag.baslik = "KATEGORİLER";
                        foreach (var item in _topRepo.FindEntities(x => !x.IsDeleted).Select(x => x.NameTR))
                        {
                            topcateDtos.Add(new FooterTopCategoryDto
                            {
                                Name = item,
                                TargetHref = $"/tr/kategoriler/{item.ConvertToFriendlyUrl()}/sayfa/1"
                            });
                        }
                        break;
                    case "en":
                        ViewBag.baslik = "CATEGORIES";
                        foreach (var item in _topRepo.FindEntities(x => !x.IsDeleted).Select(x => x.NameEN))
                        {
                            topcateDtos.Add(new FooterTopCategoryDto
                            {
                                Name = item,
                                TargetHref = $"/en/categories/{item.ConvertToFriendlyUrl()}/page/1"
                            });
                        }
                        break;
                    case "ru":
                        ViewBag.baslik = "КАТЕГОРИИ";
                        var ruscates = _topRepo.FindEntities(x => !x.IsDeleted).Select(x => new { x.NameRU, x.NameEN }).ToList();
                        foreach (var item in ruscates)
                        {
                            topcateDtos.Add(new FooterTopCategoryDto
                            {
                                Name = item.NameRU,
                                TargetHref = $"/ru/categories/{item.NameEN.ConvertToFriendlyUrl()}/page/1"
                            });
                        }
                        break;
                    default:
                        ViewBag.baslik = "CATEGORIES";
                        foreach (var item in _topRepo.FindEntities(x => !x.IsDeleted).Select(x => x.NameEN))
                        {
                            topcateDtos.Add(new FooterTopCategoryDto
                            {
                                Name = item,
                                TargetHref = $"/en/categories/{item.ConvertToFriendlyUrl()}/page/1"
                            });
                        }
                        break;
                }

                return View(topcateDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "FooterTopCategoriesComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
            
        }
    }
}
