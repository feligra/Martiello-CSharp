using Martiello.Domain.Entity;
using Martiello.Domain.Interface.Repository;
using Martiello.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Martiello.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _customers;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(DbContext context, ILogger<CustomerRepository> logger)
        {
            _customers = context.Customers;
            _logger = logger;
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            try
            {
                await _customers.InsertOneAsync(customer);
                _logger.LogInformation("Customer with ID {Id} created successfully.", customer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating customer with ID {Id}.", customer.Id);
                throw;
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(string id)
        {
            try
            {
                Customer? customer = await _customers.Find(c => c.Id == id && c.Active).FirstOrDefaultAsync();
                if (customer == null)
                {
                    _logger.LogWarning("Active customer with ID {Id} not found.", id);
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving active customer with ID {Id}.", id);
                throw;
            }
        }

        public async Task<Customer> GetCustomerByDocumentAsync(long document)
        {
            try
            {
                Customer? customer = await _customers.Find(c => c.Document == document && c.Active).FirstOrDefaultAsync();
                if (customer == null)
                {
                    _logger.LogWarning("Active customer with document {Document} not found.", document);
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving active customer with document {Document}.", document);
                throw;
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await _customers.Find(c => c.Active).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving active customers.");
                throw;
            }
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                customer.UpdatedAt = DateTime.UtcNow;

                ReplaceOneResult result = await _customers.ReplaceOneAsync(c => c.Id == customer.Id, customer);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    _logger.LogInformation("Customer with ID {Id} updated successfully.", customer.Id);
                    return true;
                }

                _logger.LogWarning("No changes were made to customer with ID {Id}.", customer.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating customer with ID {Id}.", customer.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            try
            {
                UpdateDefinition<Customer> update = Builders<Customer>.Update
                    .Set(c => c.Active, false)
                    .Set(c => c.UpdatedAt, DateTime.UtcNow);

                UpdateResult result = await _customers.UpdateOneAsync(c => c.Id == id, update);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    _logger.LogInformation("Customer with ID {Id} deactivated successfully.", id);
                    return true;
                }

                _logger.LogWarning("Customer with ID {Id} not found for deactivation.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deactivating customer with ID {Id}.", id);
                throw;
            }
        }
    }
}
