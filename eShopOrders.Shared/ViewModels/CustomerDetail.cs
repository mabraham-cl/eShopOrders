using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared.ViewModels
{
    /// <summary>
    /// Holds Customer detail
    /// </summary>
    public class CustomerDetail : CustomerName
    {
        public string Email { get; set; }

        public string CustomerId { get; set; }

        public string WebSite { get; set; }        

        public string LastLoggedIn { get; set; }

        public string HouseNumber { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string PreferredLanguage { get; set; }

    }
}
