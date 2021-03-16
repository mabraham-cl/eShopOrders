using eShopOrders.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// Public interface to access OrderService
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// This gets the most recent order for a customer
        /// </summary>
        /// <param name="customerId">Unique Identifier of the customer</param>
        /// <returns>Details of most recent order</returns>
        Task<OrderDetail> GetOrderDetailAsync(string customerId);
    }
}
