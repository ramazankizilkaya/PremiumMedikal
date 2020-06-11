using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedikalMarket.UI.Data.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(MedikalMarketContext context) : base(context) { }
    }
}
