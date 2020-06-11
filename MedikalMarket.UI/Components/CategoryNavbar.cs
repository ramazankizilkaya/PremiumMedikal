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
    public class CategoryNavbar : ViewComponent
    {
        private readonly ITopCategoryRepository _topCategoryRepo;
        private readonly IErrorLogRepository _errorRepo;

        public CategoryNavbar(ITopCategoryRepository topCategoryRepo, IErrorLogRepository errorRepo)
        {
            _topCategoryRepo = topCategoryRepo;
            _errorRepo = errorRepo;
        }

        public  IViewComponentResult Invoke()
        {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            string culture = locale.RequestCulture.UICulture.ToString();

            try
            {
                var topCatesDb = _topCategoryRepo.GetAllTopCates().ToList();
                var topCateDtos = new List<TopCategoryComponentDto>();
                ViewBag.culture = culture;

                switch (culture)
                {
                    case "tr":
                        foreach (var topCate in topCatesDb.OrderBy(x=>x.NameTR))
                        {
                            TopCategoryComponentDto topDto = new TopCategoryComponentDto();
                            topDto.Id = topCate.Id;
                            topDto.Name = topCate.NameTR;
                            topDto.HeadDescription = topCate.HeadDescriptionTR;
                            topDto.HeadTitle = topCate.HeadTitleTR;
                            topDto.TopCategoryNameUrl = topCate.TopCategoryNameUrlTR;
                            if (topCate.MiddleCategories.Any(x=>!x.IsDeleted))
                            {
                                foreach (var middleCate in topCate.MiddleCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameTR))
                                {
                                    MiddleCategoryComponentDto middleDto = new MiddleCategoryComponentDto();
                                    middleDto.Id = middleCate.Id;
                                    middleDto.Name = middleCate.NameTR;
                                    middleDto.HeadDescription = middleCate.HeadDescriptionTR;
                                    middleDto.HeadTitle = middleCate.HeadTitleTR;
                                    middleDto.TopCategoryId = topCate.Id;
                                    middleDto.MiddleCategoryNameUrl = middleCate.MiddleCategoryNameUrlTR;
                                    if (middleCate.SubCategories.Any(x => !x.IsDeleted))
                                    {
                                        foreach (var subCate in middleCate.SubCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameTR))
                                        {
                                            SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                            subDto.Id = subCate.Id;
                                            subDto.Name = subCate.NameTR;
                                            subDto.HeadDescription = subCate.HeadDescriptionTR;
                                            subDto.HeadTitle = subCate.HeadTitleTR;
                                            subDto.MiddleCategoryId = middleCate.Id;
                                            subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlTR;
                                            middleDto.SubCategoryComponentDtos.Add(subDto);
                                        }
                                    }

                                    topDto.MiddleCategoryComponentDtos.Add(middleDto);
                                }

                            }

                            topCateDtos.Add(topDto);
                        }
                        break;

                    case "en":
                        foreach (var topCate in topCatesDb.OrderBy(x => x.NameEN))
                        {
                            TopCategoryComponentDto topDto = new TopCategoryComponentDto();
                            topDto.Id = topCate.Id;
                            topDto.Name = topCate.NameEN;
                            topDto.HeadDescription = topCate.HeadDescriptionEN;
                            topDto.HeadTitle = topCate.HeadTitleEN;
                            topDto.TopCategoryNameUrl = topCate.TopCategoryNameUrlEN;
                            if (topCate.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var middleCate in topCate.MiddleCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameEN))
                                {
                                    MiddleCategoryComponentDto middleDto = new MiddleCategoryComponentDto();
                                    middleDto.Id = middleCate.Id;
                                    middleDto.Name = middleCate.NameEN;
                                    middleDto.HeadDescription = middleCate.HeadDescriptionEN;
                                    middleDto.HeadTitle = middleCate.HeadTitleEN;
                                    middleDto.TopCategoryId = topCate.Id;
                                    middleDto.MiddleCategoryNameUrl = middleCate.MiddleCategoryNameUrlEN;

                                    if (middleCate.SubCategories.Any(x => !x.IsDeleted))
                                    {
                                        foreach (var subCate in middleCate.SubCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameEN))
                                        {
                                            SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                            subDto.Id = subCate.Id;
                                            subDto.Name = subCate.NameEN;
                                            subDto.HeadDescription = subCate.HeadDescriptionEN;
                                            subDto.HeadTitle = subCate.HeadTitleEN;
                                            subDto.MiddleCategoryId = middleCate.Id;
                                            subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlEN;
                                            middleDto.SubCategoryComponentDtos.Add(subDto);
                                        }
                                    }

                                    topDto.MiddleCategoryComponentDtos.Add(middleDto);
                                }

                            }

                            topCateDtos.Add(topDto);
                        }
                        break;

                    case "ru":
                        foreach (var topCate in topCatesDb.OrderBy(x => x.NameRU))
                        {
                            TopCategoryComponentDto topDto = new TopCategoryComponentDto();
                            topDto.Id = topCate.Id;
                            topDto.Name = topCate.NameRU;
                            topDto.HeadDescription = topCate.HeadDescriptionRU;
                            topDto.HeadTitle = topCate.HeadTitleRU;
                            topDto.TopCategoryNameUrl = topCate.TopCategoryNameUrlRU;

                            if (topCate.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var middleCate in topCate.MiddleCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameRU))
                                {
                                    MiddleCategoryComponentDto middleDto = new MiddleCategoryComponentDto();
                                    middleDto.Id = middleCate.Id;
                                    middleDto.Name = middleCate.NameRU;
                                    middleDto.HeadDescription = middleCate.HeadDescriptionRU;
                                    middleDto.HeadTitle = middleCate.HeadTitleRU;
                                    middleDto.TopCategoryId = topCate.Id;
                                    middleDto.MiddleCategoryNameUrl = middleCate.MiddleCategoryNameUrlRU;

                                    if (middleCate.SubCategories.Any(x => !x.IsDeleted))
                                    {
                                        foreach (var subCate in middleCate.SubCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameRU))
                                        {
                                            SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                            subDto.Id = subCate.Id;
                                            subDto.Name = subCate.NameRU;
                                            subDto.HeadDescription = subCate.HeadDescriptionRU;
                                            subDto.HeadTitle = subCate.HeadTitleRU;
                                            subDto.MiddleCategoryId = middleCate.Id;
                                            subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlRU;
                                            middleDto.SubCategoryComponentDtos.Add(subDto);
                                        }
                                    }

                                    topDto.MiddleCategoryComponentDtos.Add(middleDto);
                                }

                            }

                            topCateDtos.Add(topDto);
                        }
                        break;
                    default:
                        foreach (var topCate in topCatesDb.OrderBy(x => x.NameEN))
                        {
                            TopCategoryComponentDto topDto = new TopCategoryComponentDto();
                            topDto.Id = topCate.Id;
                            topDto.Name = topCate.NameEN;
                            topDto.HeadDescription = topCate.HeadDescriptionEN;
                            topDto.HeadTitle = topCate.HeadTitleEN;
                            topDto.TopCategoryNameUrl = topCate.TopCategoryNameUrlEN;

                            if (topCate.MiddleCategories.Any(x => !x.IsDeleted))
                            {
                                foreach (var middleCate in topCate.MiddleCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameEN))
                                {
                                    MiddleCategoryComponentDto middleDto = new MiddleCategoryComponentDto();
                                    middleDto.Id = middleCate.Id;
                                    middleDto.Name = middleCate.NameEN;
                                    middleDto.HeadDescription = middleCate.HeadDescriptionEN;
                                    middleDto.HeadTitle = middleCate.HeadTitleEN;
                                    middleDto.TopCategoryId = topCate.Id;
                                    middleDto.MiddleCategoryNameUrl = middleCate.MiddleCategoryNameUrlEN;

                                    if (middleCate.SubCategories.Any(x => !x.IsDeleted))
                                    {
                                        foreach (var subCate in middleCate.SubCategories.Where(x => !x.IsDeleted).OrderBy(x => x.NameEN))
                                        {
                                            SubCategoryComponentDto subDto = new SubCategoryComponentDto();
                                            subDto.Id = subCate.Id;
                                            subDto.Name = subCate.NameEN;
                                            subDto.HeadDescription = subCate.HeadDescriptionEN;
                                            subDto.HeadTitle = subCate.HeadTitleEN;
                                            subDto.MiddleCategoryId = middleCate.Id;
                                            subDto.SubCategoryNameUrl = subCate.SubCategoryNameUrlEN;
                                            middleDto.SubCategoryComponentDtos.Add(subDto);
                                        }
                                    }

                                    topDto.MiddleCategoryComponentDtos.Add(middleDto);
                                }

                            }

                            topCateDtos.Add(topDto);
                        }
                        break;
                }
                return View(topCateDtos);
            }
            catch (Exception e)
            {
                _errorRepo.CreateEntity(new ErrorLog
                {
                    ErrorLocation = "CategoryNavbarComponent",
                    ErrorDetail = e.Message + " , " + e.InnerException,
                    ErrorUrl = HttpContext.Request.Path,
                    Culture=culture
                });
                return null;
            }
        }
    }
}
