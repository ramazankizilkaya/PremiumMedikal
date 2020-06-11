using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        SubCategory GetSubCateByUrlTR(string subCateNameUrl, int midcateId, int topcateId);
        SubCategory GetSubCateByUrlEN(string subCateNameUrl, int midcateId, int topcateId);
        SubCategory GetSubCateByUrlRU(string subCateNameUrl, int midcateId, int topcateId);
        SubCategory GetSubCateByNameTR(string name);
        SubCategory GetSubCateByNameEN(string name);
        SubCategory GetSubCateByNameRU(string name);
        IQueryable<SubCategory> GetSubCatesWithJoin();
        SubCategory GetSubCateByIdWithJoin(int id);
    }
}
