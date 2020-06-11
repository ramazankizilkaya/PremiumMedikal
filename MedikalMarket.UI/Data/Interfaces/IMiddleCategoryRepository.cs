using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface IMiddleCategoryRepository : IRepository<MiddleCategory>
    {
        IEnumerable<MiddleCategory> GetAllMidCates();
        MiddleCategory GetMidCateById(int id);
        MiddleCategory GetMidCateByUrlTR(string midcateNameUrl, int topcateId);
        MiddleCategory GetMidCateByUrlEN(string midcateNameUrl, int topcateId);
        MiddleCategory GetMidCateByUrlRU(string midcateNameUrl, int topcateId);
        MiddleCategory GetMidCateByNameTR(string name);
        MiddleCategory GetMidCateByNameEN(string name);
        MiddleCategory GetMidCateByNameRU(string name);
    }
}
