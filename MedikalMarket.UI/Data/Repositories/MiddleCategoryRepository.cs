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
    public class MiddleCategoryRepository : Repository<MiddleCategory>, IMiddleCategoryRepository
    {
        public MiddleCategoryRepository(MedikalMarketContext context) : base(context) { }

        public MiddleCategory GetMidCateById(int id)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.Id == id)
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public MiddleCategory GetMidCateByNameTR(string name)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.NameTR.Equals(name))
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public MiddleCategory GetMidCateByNameEN(string name)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.NameEN.Equals(name))
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public MiddleCategory GetMidCateByNameRU(string name)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.NameRU.Equals(name))
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public IEnumerable<MiddleCategory> GetAllMidCates()
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted)
                           .Include(x => x.TopCategory)
                           .Include(x => x.SubCategories)
                           .AsNoTracking()
                           .ToList();
        }

        public MiddleCategory GetMidCateByUrlTR(string midcateNameUrl, int topcateId)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.MiddleCategoryNameUrlTR.Equals(midcateNameUrl) && x.TopCategoryId==topcateId)
                            .Include(x => x.TopCategory)
                            .Include(x => x.SubCategories)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public MiddleCategory GetMidCateByUrlEN(string midcateNameUrl, int topcateId)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.MiddleCategoryNameUrlEN.Equals(midcateNameUrl) && x.TopCategoryId == topcateId)
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public MiddleCategory GetMidCateByUrlRU(string midcateNameUrl, int topcateId)
        {
            return _context.MiddleCategories.Where(x => !x.IsDeleted && x.MiddleCategoryNameUrlRU.Equals(midcateNameUrl) && x.TopCategoryId == topcateId)
                          .Include(x => x.TopCategory)
                          .Include(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }
    }
}
