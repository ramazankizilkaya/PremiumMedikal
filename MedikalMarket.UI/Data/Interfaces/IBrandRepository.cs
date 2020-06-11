using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Brand GetBrandByName(string name);
        Brand GetBrandByIdFullJoin(int id);
        IQueryable<Brand> GetAllBrandsWithFullJoin();
    }
}
