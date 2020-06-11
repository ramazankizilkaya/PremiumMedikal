using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface ITopCategoryRepository : IRepository<TopCategory>
    {
        IEnumerable<TopCategory> GetAllTopCates();
        TopCategory GetTopCateById(int id);
        TopCategory GetTopCateByUrlTR(string topcateNameUrl);
        TopCategory GetTopCateByUrlEN(string topcateNameUrl);
        TopCategory GetTopCateByUrlRU(string topcateNameUrl);
        TopCategory GetTopCateByNameTR(string name);
        TopCategory GetTopCateByNameEN(string name);
        TopCategory GetTopCateByNameRU(string name);
    }
}
