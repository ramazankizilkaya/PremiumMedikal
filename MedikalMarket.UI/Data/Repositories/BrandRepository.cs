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
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(MedikalMarketContext context) : base(context) { }

        public Brand GetBrandByName(string name)
        {
            return _context.Brands.Where(x => !x.IsDeleted && x.BrandName.Equals(name))
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public Brand GetBrandByIdFullJoin(int id)
        {
            return _context.Brands.Where(x => x.Id==id)
                            .Include(x=>x.Products)
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public IQueryable<Brand> GetAllBrandsWithFullJoin()
        {
            return _context.Brands
                            .Include(x => x.Products)
                            .AsNoTracking()
                            .AsQueryable();
        }
    }
}
