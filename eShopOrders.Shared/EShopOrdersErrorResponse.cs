using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared
{
    public class EShopOrdersErrorResponse : Exception
    {
        /// <summary>
        /// Custom Exception that can be thrown from API
        /// </summary>
        /// <param name="message">Message to the user</param>
        /// <param name="httpStatusCode">Http status code</param>
        public EShopOrdersErrorResponse(string message, int httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// HTTPS Status Code
        /// </summary>
        public int HttpStatusCode { get; set; }
    }
}
