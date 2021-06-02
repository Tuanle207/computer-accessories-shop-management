﻿using OoadProject.Core.AppSession;
using OoadProject.Core.ViewModels.Customers.Dtos;
using OoadProject.Core.ViewModels.Customers.Dtos;
using OoadProject.Core.ViewModels.Customers.Dtos;
using OoadProject.Core.ViewModels.Products.Dtos;
using OoadProject.Data.Entity.AppCustomer;
using OoadProject.Data.Repository;
using OoadProject.Shared.Dtos;
using OoadProject.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoadProject.Core.Services.AppCustomer
{
    public class CustomerService : BaseService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerLevelRepository _customerLevelRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _customerLevelRepository = new CustomerLevelRepository();
        }
        public PaginatedList<CustomerDisplayDto> GetCustomersForDisplayCustomer(int page = 1, int limit = 15, CustomerFilterDto Filter = null)
        {
            var rawCustomers = _customerRepository.GetCustomers(page, limit, Filter);

            var customersForReturn = new PaginatedList<CustomerDisplayDto>
            (
                Mapper.Map<List<CustomerDisplayDto>>(rawCustomers.Data),
                rawCustomers.TotalRecords,
                rawCustomers.CurrentPage,
                rawCustomers.PageRecords
            );
            return customersForReturn;
        }

        public Customer GetCustomer(int id)
        {
            return _customerRepository.Get(id);
        }

        public Customer GetCustomerByPhone(string phone)
        {
            return _customerRepository.GetCustomByPhoneNumber(phone);
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }


        public Customer AddCustomer(CustomerForCreationDto userForCreation)
        {
            var newCustomer = Mapper.Map<Customer>(userForCreation);

            return _customerRepository.Create(newCustomer);
        }

        public void UpdateCustomer(CustomerDisplayDto userForUpdate)
        {

            var user = Mapper.Map<Customer>(userForUpdate);
            user.Id = (int)userForUpdate.Id;

            _customerRepository.Update(user);
        }
        public bool HidenCustomer(CustomerDisplayDto customer)
        {
            var hidenCustomer = Mapper.Map<Customer>(customer);
            hidenCustomer.IsDeleted = true;
            return _customerRepository.Update(hidenCustomer);
        }
        public bool DeleteCustomer(Customer user)
        {
            return _customerRepository.Delete(user.Id);
        }

    }
}
