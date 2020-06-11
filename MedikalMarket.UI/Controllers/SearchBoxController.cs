using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedikalMarket.UI.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MedikalMarket.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerchBoxController : ControllerBase
    {
        private readonly ITopCategoryRepository _topRepo;
        private readonly IMiddleCategoryRepository _midRepo;
        private readonly ISubCategoryRepository _subRepo;
        private readonly IProductRepository _productRepo;
        private readonly IErrorLogRepository _errorRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBrandRepository _brandRepo;

        public SerchBoxController(ITopCategoryRepository topRepo, IProductRepository productRepo, IErrorLogRepository errorRepo, IHttpContextAccessor httpContextAccessor, IMiddleCategoryRepository midRepo, ISubCategoryRepository subRepo, IBrandRepository brandRepo)
        {
            _topRepo = topRepo;
            _productRepo = productRepo;
            _errorRepo = errorRepo;
            _httpContextAccessor = httpContextAccessor;
            _midRepo = midRepo;
            _subRepo = subRepo;
            _brandRepo = brandRepo;
        }


        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                string culture = locale.RequestCulture.UICulture.ToString();
                List<string> searchItems = new List<string>();
                string term = HttpContext.Request.Query["term"].ToString();

                var topCateNames = culture.Equals("tr") ? _topRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _topRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _topRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var midCateNames = culture.Equals("tr") ? _midRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _midRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _midRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var subCateNames = culture.Equals("tr") ? _subRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _subRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _subRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                var brandNames = _brandRepo.FindEntities(x => !x.IsDeleted && x.BrandName.Contains(term)).Select(x => x.BrandName).ToList();

                var products = culture.Equals("tr") ? _productRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).Select(x => x.NameTR).ToList() : culture.Equals("ru") ? _productRepo.FindEntities(x => !x.IsDeleted && x.NameRU.Contains(term)).Select(x => x.NameRU).ToList() : _productRepo.FindEntities(x => !x.IsDeleted && x.NameEN.Contains(term)).Select(x => x.NameEN).ToList();

                searchItems.AddRange(topCateNames);
                searchItems.AddRange(midCateNames);
                searchItems.AddRange(subCateNames);
                searchItems.AddRange(brandNames);
                searchItems.AddRange(products);

                return Ok(searchItems);
            }
            catch
            {
                return BadRequest();
            }
            
        }


        [Produces("application/json")]
        [HttpGet("searchProductName")]
        public async Task<IActionResult> SearchProductName()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var products = await Task.Run(() => _productRepo.FindEntities(x => !x.IsDeleted && x.NameTR.Contains(term)).ToList()).ConfigureAwait(false);
                return Ok(products);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}