using MedikalMarket.UI.Data.Interfaces;
using MedikalMarket.UI.Database.Context;
using MedikalMarket.UI.Database.Models;

namespace MedikalMarket.UI.Data.Repositories
{
    public class ContactUsRepository : Repository<ContactUs>, IContactUsRepository
    {
        public ContactUsRepository(MedikalMarketContext context) : base(context) { }
    }
}
