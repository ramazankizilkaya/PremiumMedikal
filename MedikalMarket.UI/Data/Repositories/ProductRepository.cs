using MedikalMarket.UI.Data.Enums;
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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MedikalMarketContext _context;

        public ProductRepository(MedikalMarketContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProForSliders(ProductOfferType type)
        {

            return _context.Products.Where(x => !x.IsDeleted && x.ProductOfferType == type)
                               .Include(x => x.Brand)
                               .Include(x => x.ProductPhotos)
                               .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                               .AsNoTracking();
        }

        public IQueryable<Product> GetProductsWithTopcateId(int topCateId)
        {
            return _context.Products.Where(x => !x.IsDeleted && x.TopCategoryId == topCateId)
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking();
        }

        public IQueryable<Product> GetProductsWithMidcateId(int midCateId) 
        {
            return _context.Products.Where(x => !x.IsDeleted && x.MiddleCategoryId == midCateId)
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking();
        }
        
        public IQueryable<Product> GetProductsWithSubcateId(int subCateId)
        {
            return _context.Products.Where(x => !x.IsDeleted && x.SubCategoryId == subCateId)
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking();
        }

        public IQueryable<Product> GetProductsWithBrandId(int brandId)
        {
            return _context.Products.Where(x => !x.IsDeleted && x.BrandId == brandId)
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking();
        }

        public Product GetProductWithNameUrl(string productNameUrl, string culture)
        {
            if (culture.Equals("tr"))
            {
                return _context.Products.Where(x => !x.IsDeleted && x.ProductNameUrlTR.Equals(productNameUrl))
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking()
                              .SingleOrDefault();
            }
            else if (culture.Equals("ru"))
            {
                return _context.Products.Where(x => !x.IsDeleted && x.ProductNameUrlRU.Equals(productNameUrl))
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking()
                              .SingleOrDefault();
            }
            else
            {
                return _context.Products.Where(x => !x.IsDeleted && x.ProductNameUrlEN.Equals(productNameUrl))
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking()
                              .SingleOrDefault();
            }
            
        }

        public Product GetProductWithId(int id)
        {
                return _context.Products.Where(x => !x.IsDeleted && x.Id==id)
                              .Include(x => x.Brand)
                              .Include(x => x.ProductPhotos)
                              .Include(x => x.TopCategory)
                               .Include(x => x.MiddleCategory)
                               .Include(x => x.SubCategory)
                              .AsNoTracking()
                              .SingleOrDefault();
        }

        public Product GetDeactiveProductWithId(int id)
        {
            return _context.Products.Where(x => !!x.IsDeleted && x.Id == id)
                          .Include(x => x.Brand)
                          .Include(x => x.ProductPhotos)
                          .Include(x => x.TopCategory)
                           .Include(x => x.MiddleCategory)
                           .Include(x => x.SubCategory)
                          .AsNoTracking()
                          .SingleOrDefault();
        }

        public Product GetProductWithNameTR(string proName) 
        {
            return _context.Products.Where(x => !x.IsDeleted && x.NameTR.Equals(proName))
                                    .Include(x => x.Brand)
                                    .Include(x => x.ProductPhotos)
                                    .Include(x => x.TopCategory)
                                    .Include(x => x.MiddleCategory)
                                    .Include(x => x.SubCategory)
                                    .SingleOrDefault();
        }

        public Product GetProductWithNameEN(string proName)
        {
            return _context.Products.Where(x => !x.IsDeleted && x.NameEN.Equals(proName))
                                    .Include(x => x.Brand)
                                    .Include(x => x.ProductPhotos)
                                    .Include(x => x.TopCategory)
                                    .Include(x => x.MiddleCategory)
                                    .Include(x => x.SubCategory)
                                    .SingleOrDefault();
        }

        public Product GetProductWithNameRU(string proName)
        {
            return _context.Products.Where(x => !x.IsDeleted && x.NameRU.Equals(proName))
                                    .Include(x => x.Brand)
                                    .Include(x => x.ProductPhotos)
                                    .Include(x => x.TopCategory)
                                    .Include(x => x.MiddleCategory)
                                    .Include(x => x.SubCategory)
                                    .SingleOrDefault();
        }

        public IQueryable<Product> GetActiveProductsWithJoin()
        {
            return  _context.Products.Where(x => !x.IsDeleted)
                                .Include(x => x.Brand)
                                .Include(x => x.ProductPhotos)
                                .Include(x => x.TopCategory)
                                .Include(x => x.MiddleCategory)
                                .Include(x => x.SubCategory);
        }

        public IQueryable<Product> GetDeactiveProductsWithJoin()
        {
            return _context.Products.Where(x => !x.IsDeleted)
                                .Include(x => x.Brand)
                                .Include(x => x.ProductPhotos)
                                .Include(x => x.TopCategory)
                                .Include(x => x.MiddleCategory)
                                .Include(x => x.SubCategory);
        }

        public IQueryable<Product> GetDeletedProductsWithJoin()
        {
            return _context.Products.Where(x => x.IsDeleted)
                                .Include(x => x.Brand)
                                .Include(x => x.ProductPhotos)
                                .Include(x => x.TopCategory)
                                .Include(x => x.MiddleCategory)
                                .Include(x => x.SubCategory);
        }

        public int GetTotalProductCount()
        {
            return _context.Products.Count();
        }
        
    }
}
