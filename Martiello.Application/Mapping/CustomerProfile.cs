using AutoMapper;
using Martiello.Application.UseCases.Customer.CreateCustomer;
using Martiello.Domain.Entity;

namespace Martiello.Application.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerInput, Customer>();
        }
    }
}
