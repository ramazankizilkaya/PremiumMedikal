using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MedikalMarket.UI.Components
{
    public class CategorySidebar : ViewComponent
    {
        private readonly ITopCategoryRepository _topCategoryRepo;
        private readonly IMiddleCategoryRepository _middleCategoryRepo;
        private readonly ISubCategoryRepository _subCategoryRepo;
        private readonly IErrorLogRepository _errorRepo;


        public CategorySidebar(ITopCategoryRepository topCategoryRepo, IMiddleCategoryRepository middleCategoryRepo, ISubCategoryRepository subCategoryRepo, IErrorLogRepository errorRepo)
        {
            _topCategoryRepo = topCategoryRepo;
            _middleCategoryRepo = middleCategoryRepo;
            _subCategoryRepo = subCategoryRepo;
            _errorRepo = errorRepo;
        }
        
        public IViewComponentResult Invoke(string categoryType, int id)
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                CategorySidebarDto dto = new CategorySidebarDto();
                ViewBag.filter = culture.Equals("tr") ? "Filtre" : culture.Equals("ru") ? "фильтр" : "Filter";
                switch (culture)
                {
                    case "tr":
                        if (categoryType.Equals("top"))
                        {
                            var categoryTop = _topCategoryRepo.GetTopCateById(id);

                            if (categoryTop.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var midCate in categoryTop.MiddleCategories.Where(x => !x.IsDeleted))
                                {
                                    MiddleCategoryComponentDto midDto = new MiddleCategoryComponentDto();
                                    midDto.Id = midCate.Id;
                                    midDto.Name = midCate.NameTR;
                                    midDto.MiddleCategoryNameUrl = midCate.MiddleCategoryNameUrlTR;
                                    midDto.TopCategoryId = midCate.TopCategoryId;
                                    midDto.CategoryHref = $"/tr/kategoriler/{midCate.TopCategory.TopCategoryNameUrlTR}/{midCate.MiddleCategoryNameUrlTR}/sayfa/1";
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                    dto.TopCategoryId = midCate.Id;
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                }
                            }
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryTop.NameTR;
                            return View(dto);
                        }
                        else if (categoryType.Equals("mid"))
                        {
                            var categoryMid = _middleCategoryRepo.GetMidCateById(id);

                            if (categoryMid.SubCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var subCate in categoryMid.SubCategories.Where(x => !x.IsDeleted))
                                {
                                    SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                    subDto.Id = subCate.Id;
                                    subDto.Name = subCate.NameTR;
                                    subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlTR;
                                    subDto.MiddleCategoryId = subCate.MiddleCategoryId;
                                    subDto.CategoryHref = $"/tr/kategoriler/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlTR}/{subCate.MiddleCategory.MiddleCategoryNameUrlTR}/{subCate.SubCategoryNameUrlTR}/sayfa/1";
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                    dto.TopCategoryId = categoryMid.TopCategoryId;
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                }
                            }
                            
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryMid.NameTR;
                            return View(dto);
                        }
                        else
                        {
                            var categorySub = _subCategoryRepo.FindEntity(x => !x.IsDeleted && x.Id == id);
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categorySub.NameTR;
                            return View(dto);
                        }
                    case "en":
                        if (categoryType.Equals("top"))
                        {
                            var categoryTop = _topCategoryRepo.GetTopCateById(id);
                            if (categoryTop.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var midCate in categoryTop.MiddleCategories.Where(x => !x.IsDeleted))
                                {
                                    MiddleCategoryComponentDto midDto = new MiddleCategoryComponentDto();
                                    midDto.Id = midCate.Id;
                                    midDto.Name = midCate.NameEN;
                                    midDto.MiddleCategoryNameUrl = midCate.MiddleCategoryNameUrlEN;
                                    midDto.TopCategoryId = midCate.TopCategoryId;
                                    midDto.CategoryHref = $"/en/categories/{midCate.TopCategory.TopCategoryNameUrlEN}/{midCate.MiddleCategoryNameUrlEN}/page/1";
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                    dto.TopCategoryId = midCate.Id;
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                }
                            }
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryTop.NameEN;
                            return View(dto);
                        }
                        else if (categoryType.Equals("mid"))
                        {
                            var categoryMid = _middleCategoryRepo.GetMidCateById(id);
                            if (categoryMid.SubCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var subCate in categoryMid.SubCategories.Where(x => !x.IsDeleted))
                                {
                                    SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                    subDto.Id = subCate.Id;
                                    subDto.Name = subCate.NameEN;
                                    subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlEN;
                                    subDto.MiddleCategoryId = subCate.MiddleCategoryId;
                                    subDto.CategoryHref = $"/en/categories/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlEN}/{subCate.MiddleCategory.MiddleCategoryNameUrlEN}/{subCate.SubCategoryNameUrlEN}/page/1";
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                    dto.TopCategoryId = categoryMid.TopCategoryId;
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                }
                            }
                            
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryMid.NameEN;
                            return View(dto);
                        }
                        else
                        {
                            var categorySub = _subCategoryRepo.FindEntity(x => !x.IsDeleted && x.Id == id);
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categorySub.NameEN;
                            return View(dto);
                        }

                    case "ru":
                        if (categoryType.Equals("top"))
                        {
                            var categoryTop = _topCategoryRepo.GetTopCateById(id);
                            if (categoryTop.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var midCate in categoryTop.MiddleCategories.Where(x => !x.IsDeleted))
                                {
                                    MiddleCategoryComponentDto midDto = new MiddleCategoryComponentDto();
                                    midDto.Id = midCate.Id;
                                    midDto.Name = midCate.NameRU;
                                    midDto.MiddleCategoryNameUrl = midCate.MiddleCategoryNameUrlRU;
                                    midDto.TopCategoryId = midCate.TopCategoryId;
                                    midDto.CategoryHref = $"/ru/categories/{midCate.TopCategory.TopCategoryNameUrlRU}/{midCate.MiddleCategoryNameUrlRU}/page/1";
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                    dto.TopCategoryId = midCate.Id;
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                }
                            }
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryTop.NameRU;
                            return View(dto);
                        }
                        else if (categoryType.Equals("mid"))
                        {
                            var categoryMid = _middleCategoryRepo.GetMidCateById(id);
                            if (categoryMid.SubCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var subCate in categoryMid.SubCategories.Where(x => !x.IsDeleted))
                                {
                                    SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                    subDto.Id = subCate.Id;
                                    subDto.Name = subCate.NameRU;
                                    subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlRU;
                                    subDto.MiddleCategoryId = subCate.MiddleCategoryId;
                                    subDto.CategoryHref = $"/ru/categories/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlRU}/{subCate.MiddleCategory.MiddleCategoryNameUrlRU}/{subCate.SubCategoryNameUrlRU}/page/1";
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                    dto.TopCategoryId = categoryMid.TopCategoryId;
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                }
                            }
                           
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryMid.NameRU;
                            return View(dto);
                        }
                        else
                        {
                            var categorySub = _subCategoryRepo.FindEntity(x => !x.IsDeleted && x.Id == id);
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categorySub.NameRU;
                            return View(dto);
                        }
                    default:
                        if (categoryType.Equals("top"))
                        {
                            var categoryTop = _topCategoryRepo.GetTopCateById(id);
                            if (categoryTop.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var midCate in categoryTop.MiddleCategories.Where(x => !x.IsDeleted))
                                {
                                    MiddleCategoryComponentDto midDto = new MiddleCategoryComponentDto();
                                    midDto.Id = midCate.Id;
                                    midDto.Name = midCate.NameEN;
                                    midDto.MiddleCategoryNameUrl = midCate.MiddleCategoryNameUrlEN;
                                    midDto.TopCategoryId = midCate.TopCategoryId;
                                    midDto.CategoryHref = $"/en/categories/{midCate.TopCategory.TopCategoryNameUrlEN}/{midCate.MiddleCategoryNameUrlEN}/page/1";
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                    dto.TopCategoryId = midCate.Id;
                                    dto.MiddleCategoryComponentDtos.Add(midDto);
                                }
                            }
                           
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryTop.NameEN;
                            return View(dto);
                        }
                        else if (categoryType.Equals("mid"))
                        {
                            var categoryMid = _middleCategoryRepo.GetMidCateById(id);
                            if (categoryMid.SubCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var subCate in categoryMid.SubCategories.Where(x => !x.IsDeleted))
                                {
                                    SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                    subDto.Id = subCate.Id;
                                    subDto.Name = subCate.NameEN;
                                    subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlEN;
                                    subDto.MiddleCategoryId = subCate.MiddleCategoryId;
                                    subDto.CategoryHref = $"/en/categories/{subCate.MiddleCategory.TopCategory.TopCategoryNameUrlEN}/{subCate.MiddleCategory.MiddleCategoryNameUrlEN}/{subCate.SubCategoryNameUrlEN}/page/1";
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                    dto.TopCategoryId = categoryMid.TopCategoryId;
                                    dto.SubCategoryComponentDtos.Add(subDto);
                                }
                            }
                           
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categoryMid.NameEN;
                            return View(dto);
                        }
                        else
                        {
                            var categorySub = _subCategoryRepo.FindEntity(x => !x.IsDeleted && x.Id == id);
                            dto.CategoryType = categoryType;
                            dto.ComponentTitle = categorySub.NameEN;
                            return View(dto);
                        }
                }
                }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "CategorySidebarComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture = culture
                });
                return null;
            }
        }
    }
}
