using AutoMapper;
using MedikalMarket.UI.Areas.Admin.Dtos;
using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Data.Dtos.ComponentDtos;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Mapper
{
    public class MedikalMarketMapper:Profile
    {
        public MedikalMarketMapper()
        {
            CreateMap<Brand, AdminBrandDto>().ReverseMap();
            CreateMap<Brand, AdminCreateBrandDto>().ReverseMap();
            CreateMap<Slider, SliderDto>().ReverseMap();
            CreateMap<Slider, AdminSliderDto>().ReverseMap();
            CreateMap<Slider, AdminCreateSliderDto>().ReverseMap();
            CreateMap<MiniSlider, MiniSliderDto>().ReverseMap();
            CreateMap<MiniSlider, AdminMiniSliderDto>().ReverseMap();
            CreateMap<AdProduct, AdProductDto>().ReverseMap();
            CreateMap<AdProduct, SliderDto>().ReverseMap();
            CreateMap<AdProduct, AdminAdProductDto>().ReverseMap();
            CreateMap<AdProduct, AdminCreateAdProductDto>().ReverseMap();
            CreateMap<SmilarProductDto, Product>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Product, AdminProductDto>().ReverseMap();  
            CreateMap<TopCategory, AdminCateDto>().ReverseMap();  
            CreateMap<MiddleCategory, AdminCateDto>().ReverseMap();  
            CreateMap<SubCategory, AdminCateDto>().ReverseMap();  

        }
    }
}
