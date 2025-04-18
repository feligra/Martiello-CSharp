﻿using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerInput : IUseCaseInput
    {
        public long Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
