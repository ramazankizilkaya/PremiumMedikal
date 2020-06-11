using MedikalMarket.UI.Business.Helpers;
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
    public class LastVisitedProducts : ViewComponent
    {
        private readonly IProductRepository _productRepo;

        public LastVisitedProducts(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public IViewComponentResult Invoke()
        {
            List<LVPDto> lastVisitedProducts = new List<LVPDto>();
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            switch (culture)
            {
                case "tr":
                    foreach (var item in CustomHelpers.VisitedProducts)
                    {
                        LVPDto dto = new LVPDto();
                        var proDb = _productRepo.GetEntityById(item);
                        dto.PhotoFileName = proDb.MainPhotoFileName;
                        dto.ProductName = proDb.NameTR;
                        dto.PhotoAltTag = proDb.PhotoAltTagTR;
                        dto.ProductBrand = proDb.Brand.BrandName;
                        lastVisitedProducts.Add(dto);
                    }
                    break;
                case "en":
                    foreach (var item in CustomHelpers.VisitedProducts)
                    {
                        LVPDto dto = new LVPDto();
                        var proDb = _productRepo.GetEntityById(item);
                        dto.PhotoFileName = proDb.MainPhotoFileName;
                        dto.ProductName = proDb.NameEN;
                        dto.PhotoAltTag = proDb.PhotoAltTagEN;
                        lastVisitedProducts.Add(dto);
                        dto.ProductBrand = proDb.Brand.BrandName;
                    }
                    break;
                case "ru":
                    foreach (var item in CustomHelpers.VisitedProducts)
                    {
                        LVPDto dto = new LVPDto();
                        var proDb = _productRepo.GetEntityById(item);
                        dto.PhotoFileName = proDb.MainPhotoFileName;
                        dto.ProductName = proDb.NameRU;
                        dto.PhotoAltTag = proDb.PhotoAltTagRU;
                        lastVisitedProducts.Add(dto);
                        dto.ProductBrand = proDb.Brand.BrandName;
                    }
                    break;
                default:
                    foreach (var item in CustomHelpers.VisitedProducts)
                    {
                        LVPDto dto = new LVPDto();
                        var proDb = _productRepo.GetEntityById(item);
                        dto.PhotoFileName = proDb.MainPhotoFileName;
                        dto.ProductName = proDb.NameEN;
                        dto.PhotoAltTag = proDb.PhotoAltTagEN;
                        lastVisitedProducts.Add(dto);
                        dto.ProductBrand = proDb.Brand.BrandName;
                    }
                    break;
            }

            lastVisitedProducts.Reverse();

            return View(lastVisitedProducts.Take(5));
        }
    }
}
