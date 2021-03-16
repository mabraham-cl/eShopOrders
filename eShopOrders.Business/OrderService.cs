using eShopOrders.Database.Models;
using eShopOrders.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// This service deals with Orders 
    /// </summary>
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public OrderService(SSE_TestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This gets the most recent order for a customer
        /// </summary>
        /// <param name="customerId">Unique Identifier of the customer</param>
        /// <returns>Details of most recent order</returns>
        public async Task<OrderDetail> GetOrderDetailAsync(string customerId)
        {
            var result = await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Orderitems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.Customerid == customerId)
                    .OrderByDescending(o => o.Orderdate)
                    .FirstOrDefaultAsync();

            if (result == null)
                return null;

            return (MapOrderItems(result));
        }

        /// <summary>
        /// This method maps the database model to the view model/business object
        /// (Alternatively can map this through Automapper)
        /// </summary>
        /// <param name="result">Result object</param>
        /// <returns>OrderDetail object</returns>
        private OrderDetail MapOrderItems(dynamic result)
        {            
            var orderDetail = new OrderDetail
            {
                DeliveryExpected = result.Deliveryexpected.ToString("d-MMM-yyyy"),
                OrderDate = result.Orderdate.ToString("d-MMM-yyyy"),
                OrderNumber = result.Orderid,
                OrderItems = new List<ProductDetail>()
            };

            foreach (dynamic orderIt in result.Orderitems)
            {
                
                var orderItem = new ProductDetail
                {
                    Product = result.Containsgift == true ? "Gift" : orderIt.Product.Productname,
                    Quantity = orderIt.Quantity,
                    PriceEach = orderIt.Price
                };

                orderDetail.OrderItems.Add(orderItem);
            }

            return orderDetail;
        }

        /// <summary>
        /// DatabaseContext object
        /// </summary>
        private readonly SSE_TestContext _context;
    }
}
