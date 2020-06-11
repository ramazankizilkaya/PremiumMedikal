using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MedikalMarket.UI.Data.Repositories
{
    public class TopCategoryRepository : Repository<TopCategory>, ITopCategoryRepository
    {
        private readonly MedikalMarketContext _context;

        public TopCategoryRepository(MedikalMarketContext context) : base(context)
        {
            _context = context;
        }

        public TopCategory GetTopCateById(int id)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.Id == id)
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public TopCategory GetTopCateByNameTR(string name)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.NameTR.Equals(name))
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public TopCategory GetTopCateByNameEN(string name)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.NameEN.Equals(name))
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public TopCategory GetTopCateByNameRU(string name)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.NameRU.Equals(name))
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public IEnumerable<TopCategory> GetAllTopCates()
        {
            return _context.TopCategories
                        .Where(x => !x.IsDeleted)
                        .Include(x => x.MiddleCategories)
                        .ThenInclude(x => x.SubCategories)
                        .AsNoTracking()
                        .ToList();
        }

        public TopCategory GetTopCateByUrlTR(string topcateNameUrl)
        {
            return _context.TopCategories
                        .Where(x => !x.IsDeleted && x.TopCategoryNameUrlTR.Equals(topcateNameUrl))
                        .Include(x => x.MiddleCategories)
                        .ThenInclude(x => x.SubCategories)
                        .AsNoTracking()
                        .SingleOrDefault();
        }

        public TopCategory GetTopCateByUrlEN(string topcateNameUrl)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.TopCategoryNameUrlEN.Equals(topcateNameUrl))
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public TopCategory GetTopCateByUrlRU(string topcateNameUrl)
        {
            return _context.TopCategories.Where(x => !x.IsDeleted && x.TopCategoryNameUrlRU.Equals(topcateNameUrl))
                          .Include(x => x.MiddleCategories)
                          .ThenInclude(x => x.SubCategories)
                          .AsNoTracking()
                          .SingleOrDefault();

        }
    }
}
