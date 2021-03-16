using eShopOrders.Shared;
using eShopOrders.Shared.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopOrders.Business
{
    /// <summary>
    /// This service deals with Customers API
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient object</param>
        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// This retrieves the Customer details by email
        /// </summary>
        /// <param name="email">Email id of customer</param>
        /// <returns>Customer Details</returns>
        public async Task<CustomerDetail> GetCustomerByEmailAsync(string email)
        {
            Uri customerUri = new Uri($"{_httpClient.BaseAddress}&email={email}");

            var responseString = await _httpClient.GetStringAsync(customerUri);

            if(string.IsNullOrEmpty(responseString))
                throw new EShopOrdersException($"The customer with email, {email} is not found.", 404);

            var customerDetail = JsonConvert.DeserializeObject<CustomerDetail>(responseString);

            return customerDetail;
        }

        /// <summary>
        /// HttpClient object
        /// </summary>
        private readonly HttpClient _httpClient;
    }
}
