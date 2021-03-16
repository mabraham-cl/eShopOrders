using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared.ViewModels
{
    /// <summary>
    /// Holds information about the Product
    /// </summary>
    public class ProductDetail
    {
        public string Product { get; set; }

        public long Quantity { get; set; }

        public decimal PriceEach { get; set; }

    }
}
