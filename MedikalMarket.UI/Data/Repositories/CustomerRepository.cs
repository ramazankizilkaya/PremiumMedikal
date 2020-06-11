using MedikalMarket.UI.Business.Helpers;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly MedikalMarketContext _context;

        public CustomerRepository(MedikalMarketContext context) : base(context) 
        {
            _context = context;
        }

        public CustomerDto GetCustomerDtoWithFPs(int id, string cult)
        {
            CustomerDto customerDto = new CustomerDto();

            List<FavoriteProductDto> fpDtoList = new List<FavoriteProductDto>();

            var customer = _context.Customers.Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);

            customer.FavoriteProducts = _context.FavoriteProducts.Where(x => !x.IsDeleted && x.CustomerId == id)
                        .Include(x => x.Product)
                        .Include(x => x.Product.TopCategory)
                        .Include(x => x.Product.MiddleCategory)
                        .Include(x => x.Product.SubCategory)
                        .Include(x => x.Product.Brand).AsQueryable().ToList();

            customerDto.Id = customer.Id;
            customerDto.NameSurname = customer.NameSurname;
            customerDto.EmailAddress = customer.EmailAddress;
            customerDto.Password = customer.Password;
            customerDto.CellPhoneNumber = customer.CellPhoneNumber;
            customerDto.IsSubscribedToEmail = customer.IsSubscribedToEmail;
            customerDto.IsSubscribedToSMS = customer.IsSubscribedToSMS;
            customerDto.Address = customer.Address;

            if (_context.FavoriteProducts.Count(x => !x.IsDeleted && x.CustomerId == id) != 0)
            {
                foreach (var item in customer.FavoriteProducts.ToList())
                {
                    FavoriteProductDto dto = new FavoriteProductDto
                    {
                        Id=item.Id,
                        BrandName=item.Product.Brand.BrandName,
                        BrandNameUrl=item.Product.Brand.BrandName.ConvertToFriendlyUrl(),
                        MainPhotoFileName = item.Product.MainPhotoFileName,
                        PhotoAltTag= cult.Equals("tr") ? item.Product.PhotoAltTagTR : cult.Equals("ru") ? item.Product.PhotoAltTagRU : item.Product.PhotoAltTagEN,
                        
                        CustomerId=item.Customer.Id,
                        ProductId=item.Product.Id,
                        ProductName= cult.Equals("tr") ? item.Product.NameTR : cult.Equals("ru") ? item.Product.NameRU : item.Product.NameEN,
                        ProductNameUrl = cult.Equals("tr") ? item.Product.ProductNameUrlTR : item.Product.ProductNameUrlEN,
                        
                        TopCategoryName= cult.Equals("tr") ? item.Product.TopCategory.NameTR : cult.Equals("ru") ? item.Product.TopCategory.NameRU : item.Product.TopCategory.NameEN,
                        TopCategoryNameUrl= cult.Equals("tr") ? item.Product.TopCategory.TopCategoryNameUrlTR : item.Product.TopCategory.TopCategoryNameUrlEN,
                        CreatedDate=item.CreatedDate,
                        BrandHref= cult.Equals("tr") ? $"/tr/markalar/{item.Product.Brand.BrandName.ConvertToFriendlyUrl()}/sayfa/1" : cult.Equals("ru") ? $"/ru/brands/{item.Product.Brand.BrandName.ConvertToFriendlyUrl()}/page/1" : $"/en/brands/{item.Product.Brand.BrandName.ConvertToFriendlyUrl()}/page/1"
                    };

                    if (item.Product.SubCategoryId != null && item.Product.MiddleCategoryId != null)
                    {
                        dto.SubCategoryName = cult.Equals("tr") ? item.Product.SubCategory.NameTR : cult.Equals("ru") ? item.Product.SubCategory.NameRU : item.Product.SubCategory.NameEN;
                        dto.SubCategoryNameUrl = cult.Equals("tr") ? item.Product.SubCategory.SubCategoryNameUrlTR : item.Product.SubCategory.SubCategoryNameUrlEN;

                        dto.MiddleCategoryName = cult.Equals("tr") ? item.Product.MiddleCategory.NameTR : cult.Equals("ru") ? item.Product.MiddleCategory.NameRU : item.Product.MiddleCategory.NameEN;

                        dto.MiddleCategoryNameUrl = cult.Equals("tr") ? item.Product.MiddleCategory.MiddleCategoryNameUrlTR : item.Product.MiddleCategory.MiddleCategoryNameUrlEN;

                        dto.ProductHref = cult.Equals("tr") ? $"/tr/urun-detay/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlTR}/{item.Product.MiddleCategory.MiddleCategoryNameUrlTR}/{item.Product.SubCategory.SubCategoryNameUrlTR}/{item.Product.NameTR.ConvertToFriendlyUrl()}"
                            : cult.Equals("ru") ? $"/ru/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlRU}/{item.Product.MiddleCategory.MiddleCategoryNameUrlRU}/{item.Product.SubCategory.SubCategoryNameUrlRU}/{item.Product.NameEN.ConvertToFriendlyUrl()}"
                            : $"/en/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlEN}/{item.Product.MiddleCategory.MiddleCategoryNameUrlEN}/{item.Product.SubCategory.SubCategoryNameUrlEN}/{item.Product.NameEN.ConvertToFriendlyUrl()}";
                    }
                    else if (item.Product.SubCategoryId == null && item.Product.MiddleCategoryId == null)
                    {
                        dto.ProductHref = cult.Equals("tr") ? $"/tr/urun-detay/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlTR}/{item.Product.NameTR.ConvertToFriendlyUrl()}" : cult.Equals("ru") ? $"/ru/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlRU}/{item.Product.NameEN.ConvertToFriendlyUrl()}" : $"/en/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlEN}/{item.Product.NameEN.ConvertToFriendlyUrl()}";
                    }
                    else
                    {
                        dto.MiddleCategoryName = cult.Equals("tr") ? item.Product.MiddleCategory.NameTR : cult.Equals("ru") ? item.Product.MiddleCategory.NameRU : item.Product.MiddleCategory.NameEN;

                        dto.MiddleCategoryNameUrl = cult.Equals("tr") ? item.Product.MiddleCategory.MiddleCategoryNameUrlTR : item.Product.MiddleCategory.MiddleCategoryNameUrlEN;

                        dto.ProductHref = cult.Equals("tr") ? $"/tr/urun-detay/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlTR}/{item.Product.MiddleCategory.MiddleCategoryNameUrlTR}/{item.Product.NameTR.ConvertToFriendlyUrl()}" : cult.Equals("ru") ? $"/ru/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlRU}/{item.Product.MiddleCategory.MiddleCategoryNameUrlRU}/{item.Product.NameEN.ConvertToFriendlyUrl()}" : $"/en/product-detail/{item.Product.Brand.BandNameUrl}/{item.Product.TopCategory.TopCategoryNameUrlEN}/{item.Product.MiddleCategory.MiddleCategoryNameUrlEN}/{item.Product.NameEN.ConvertToFriendlyUrl()}";
                    }
                    fpDtoList.Add(dto);
                }

            }

            customerDto.FavoriteProductDtos = fpDtoList;

            return customerDto;
        }
    }
}
