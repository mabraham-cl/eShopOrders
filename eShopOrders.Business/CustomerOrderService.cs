using eShopOrders.Shared;
using eShopOrders.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerOrderService : ICustomerOrderService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerService"> Interface object of CustomerService</param>
        /// <param name="orderService">Interface object of OrderService</param>
        public CustomerOrderService(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }

        /// <summary>
        /// Gets the most recent customer order details
        /// </summary>
        /// <param name="user">Email of the user</param>
        /// <param name="customerId">Customer Id</param>
        /// <returns>Most recent customer order details</returns>
        public async Task<CustomerOrder> GetCustomerOrderAsync(string user, string customerId)
        {
            if(string.IsNullOrEmpty(user) || string.IsNullOrWhiteSpace(user))
                throw new EShopOrdersErrorResponse($"Please provide a valid email id.", 400);

            if (string.IsNullOrEmpty(customerId) || string.IsNullOrWhiteSpace(customerId))
                throw new EShopOrdersErrorResponse($"Please provide a valid customer id.", 400);

            var customerDetail = await _customerService.GetCustomerByEmailAsync(user);

            if (customerDetail.CustomerId != customerId)
                throw new EShopOrdersErrorResponse($"user's email address {user} does not match the customer number {customerId}", 400);

            var orderDetail = await _orderService.GetOrderDetailAsync(customerId);

            CustomerOrder customerOrder = new CustomerOrder()
            { 
                Customer =new CustomerName() {FirstName = customerDetail .FirstName, LastName = customerDetail.LastName},
                Order = orderDetail
            };

            if(customerOrder.Order !=null )
                customerOrder.Order.DeliveryAddress = $"{customerDetail.HouseNumber}, {customerDetail.Street}, {customerDetail.Town}, {customerDetail.PostCode}";

            return customerOrder;
        }

        /// <summary>
        /// Interface object of CustomerService
        /// </summary>
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Interface object of OrderService
        /// </summary>
        private readonly IOrderService _orderService;
    }
}
