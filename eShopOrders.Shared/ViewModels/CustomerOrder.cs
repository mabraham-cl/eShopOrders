using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared.ViewModels
{
    /// <summary>
    /// Holds information about Customer and Order
    /// </summary>
    public class CustomerOrder
    {
        public CustomerName Customer { get; set; }

        public OrderDetail Order { get; set; }
    }
}
