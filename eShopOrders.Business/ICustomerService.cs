using eShopOrders.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// Public interface to access CustomerService
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// This retrieves the Customer details by email
        /// </summary>
        /// <param name="email">Email id of customer</param>
        /// <returns>Customer Details</returns>
        Task<CustomerDetail> GetCustomerByEmailAsync(string email);
    }
}
