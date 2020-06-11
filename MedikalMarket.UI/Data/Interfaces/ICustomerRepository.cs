using MedikalMarket.UI.Data.Dtos;
using MedikalMarket.UI.Database.Models;

namespace MedikalMarket.UI.Data.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        CustomerDto GetCustomerDtoWithFPs(int id, string cult);
    }
}
