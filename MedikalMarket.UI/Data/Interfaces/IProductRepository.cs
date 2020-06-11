using MedikalMarket.UI.Data.Enums;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        IQueryable<Product> GetProForSliders(ProductOfferType type);
        IQueryable<Product> GetProductsWithTopcateId(int topCateId);
        IQueryable<Product> GetProductsWithMidcateId(int midCateId);
        IQueryable<Product> GetProductsWithSubcateId(int subCateId);
        IQueryable<Product> GetProductsWithBrandId(int brandId);
        Product GetProductWithNameUrl(string productNameUrl, string culture);
        Product GetProductWithId(int id);
        Product GetProductWithNameTR(string proName);
        Product GetProductWithNameEN(string proName);
        Product GetProductWithNameRU(string proName);
        Product GetDeactiveProductWithId(int id);
        IQueryable<Product> GetActiveProductsWithJoin();
        IQueryable<Product> GetDeactiveProductsWithJoin();
        IQueryable<Product> GetDeletedProductsWithJoin();
        int GetTotalProductCount();
    }
}
