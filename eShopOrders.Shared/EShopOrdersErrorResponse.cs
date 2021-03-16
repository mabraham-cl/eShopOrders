using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOrders.Shared
{
    public class EShopOrdersErrorResponse
    {
        /// <summary>
        /// Message which describes the error
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// The HTTP status code of the error
        /// </summary>
        [JsonProperty("status")]
        public int HttpStatus { get; set; }

        /// <summary>
        /// A message e.g: a stack trace
        /// </summary>
        [JsonProperty("developerMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string DeveloperMessage { get; set; }
    }
}
