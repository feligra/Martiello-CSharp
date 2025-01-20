using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiello.Domain.Entity;

namespace Martiello.Domain.Interface.Repository
{
    public interface ICustomerRepository
    {
        Task CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(string id);
        Task<Customer> GetCustomerByDocumentAsync(long document);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(string id);
    }
}
