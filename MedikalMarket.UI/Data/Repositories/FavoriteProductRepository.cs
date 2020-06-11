using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Repositories
{
    public class FavoriteProductRepository : Repository<FavoriteProduct>, IFavoriteProductRepository
    {
        public FavoriteProductRepository(MedikalMarketContext context) : base(context) { }
    }
}
