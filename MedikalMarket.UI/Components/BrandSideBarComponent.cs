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

namespace MedikalMarket.UI.Components
{
    public class BrandSideBarComponent : ViewComponent
    {
        private readonly IErrorLogRepository _errorRepo;
        private readonly IBrandRepository _brandRepo;
        private readonly IStringLocalizer<AdsProductComponent> _localizer;



        public BrandSideBarComponent(IBrandRepository brandRepo, IErrorLogRepository errorRepo, IStringLocalizer<AdsProductComponent> localizer)
        {
            _errorRepo = errorRepo;
            _brandRepo = brandRepo;
            _localizer = localizer;
        }

        public IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                //satışta ürünü olmayan markaları slider dan ve side bardan sil, ya da bu markaya ait ürün yoktur yazısı eklemek daha kolay olabilir.
                ViewBag.Title = culture.Equals("tr") ? "Markalar" : culture.Equals("ru") ? "бренды" : "Brands";
                var brands = _brandRepo.GetAllBrandsWithFullJoin().Where(x => !x.IsDeleted).ToList();
                List<BrandSideBarComponentDto> dtoList = new List<BrandSideBarComponentDto>();

                if (culture.Equals("tr"))
                {
                    foreach (var item in brands.Where(x=>x.Products.Count > 0))
                    {
                        BrandSideBarComponentDto dto = new BrandSideBarComponentDto();
                        dto.BrandName = item.BrandName;
                        dto.BrandUrl = $"/tr/markalar/{item.BandNameUrl}/sayfa/1";
                        dtoList.Add(dto);
                    }
                }
                else if (culture.Equals("ru"))
                {
                    foreach (var item in brands.Where(x => x.Products.Count > 0))
                    {
                        BrandSideBarComponentDto dto = new BrandSideBarComponentDto();
                        dto.BrandName = item.BrandName;
                        dto.BrandUrl = $"/ru/brands/{item.BandNameUrl}/page/1";
                        dtoList.Add(dto);
                    }
                }
                else
                {
                    foreach (var item in brands.Where(x => x.Products.Count > 0))
                    {
                        BrandSideBarComponentDto dto = new BrandSideBarComponentDto();
                        dto.BrandName = item.BrandName;
                        dto.BrandUrl = $"/en/brands/{item.BandNameUrl}/page/1";
                        dtoList.Add(dto);
                    }
                }


                return View(dtoList);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "BrandSideBarComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                HttpContext.Session.SetString("alert", _localizer["Hata oluştu ve kayıt altına alındı."]);
                //_accesor.HttpContext.Response.Redirect("/");
                return null;

            }
            
        }
    }
}
