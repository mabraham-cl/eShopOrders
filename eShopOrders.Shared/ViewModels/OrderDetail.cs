using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared.ViewModels
{
    /// <summary>
    /// Holds information about Order
    /// </summary>
    public class OrderDetail
    {
        public long OrderNumber { get; set; }

        public string OrderDate { get; set; }

        public string DeliveryAddress { get; set; }

        public List<ProductDetail> OrderItems { get; set; }

        public string DeliveryExpected { get; set; }
    }
}
