using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Repositories
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(MedikalMarketContext context) : base(context) { }

        public SubCategory GetSubCateByUrlTR(string subCateNameUrl, int midcateId, int topcateId)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.SubCategoryNameUrlTR.Equals(subCateNameUrl) && x.MiddleCategoryId == midcateId && x.MiddleCategory.TopCategoryId == topcateId)
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public SubCategory GetSubCateByUrlEN(string subCateNameUrl, int midcateId, int topcateId)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.SubCategoryNameUrlEN.Equals(subCateNameUrl) && x.MiddleCategoryId == midcateId && x.MiddleCategory.TopCategoryId == topcateId)
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x=>x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public SubCategory GetSubCateByUrlRU(string subCateNameUrl, int midcateId, int topcateId)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.SubCategoryNameUrlRU.Equals(subCateNameUrl) && x.MiddleCategoryId == midcateId && x.MiddleCategory.TopCategoryId == topcateId)
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public IQueryable<SubCategory> GetSubCatesWithJoin()
        {
            return _context.SubCategories.Where(x => !x.IsDeleted)
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking();
        }
        
        public SubCategory GetSubCateByIdWithJoin(int id)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.Id==id)
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault(); 
        }

        public SubCategory GetSubCateByNameTR(string name)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.NameTR.Equals(name))
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public SubCategory GetSubCateByNameEN(string name)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.NameEN.Equals(name))
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public SubCategory GetSubCateByNameRU(string name)
        {
            return _context.SubCategories.Where(x => !x.IsDeleted && x.NameRU.Equals(name))
                            .Include(x => x.MiddleCategory)
                            .ThenInclude(x => x.TopCategory)
                            .AsNoTracking()
                            .SingleOrDefault();
        }
    }
}
