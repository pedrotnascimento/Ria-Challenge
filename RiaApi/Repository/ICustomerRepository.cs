using System.Collections.Generic;
using System.Threading.Tasks;
using RiaApi.Models;
namespace RiaApi.Repository
{

    public interface ICustomerRepository
    {
        Task<List<Customer>?> GetAllAsync();
        Task CreateManyAsync(IEnumerable<Customer> customers);
        Task<bool> HasExistentIdAsync(IEnumerable<Customer> customers);
        List<Customer>? GetInternalArray();
    }

}