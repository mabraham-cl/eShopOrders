using eShopOrders.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// Public interface to access CustomerOrderService
    /// </summary>
    public interface ICustomerOrderService
    {
        /// <summary>
        /// Gets the most recent customer order details
        /// </summary>
        /// <param name="user">Email of the user</param>
        /// <param name="customerId">Customer Id</param>
        /// <returns>Most recent customer order details</returns>
        Task<CustomerOrder> GetCustomerOrderAsync(string user, string customerId);
    }
}
