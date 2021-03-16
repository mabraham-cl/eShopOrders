using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using eShopOrders.Business;
using eShopOrders.Shared.ViewModels;
using Moq;
using eShopOrders.Shared;
using Newtonsoft.Json;
using System.Net.Http;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace eShopOrders.UnitTests
{
    public class CustomerOrderService_UnitTest
    {
        [Fact]
        public async Task CustomerOrderService_UnitTest_GetCustomerOrderAsync_ReturnCustomerOrderAsync()
        {
            // Arrange
            var stringCustomer = JsonConvert.SerializeObject(GetCustomerDetail());
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(stringCustomer)
                });
            Mock<HttpClient> mockHttpClient = new Mock<HttpClient>(mockMessageHandler.Object);
            mockHttpClient.Object.BaseAddress = new Uri("https://customer-details.azurewebsites.net/api/GetUserDetails");
            var customerService = new CustomerService(mockHttpClient.Object);

            // Act
            var result = await customerService.GetCustomerByEmailAsync("bob@mmtdigital.co.uk");

            // Assert
            var custDetail = Assert.IsType<CustomerDetail>(result);

            Assert.Equal("R223232", custDetail.CustomerId);
            Assert.Equal("bob@mmtdigital.co.uk", custDetail.Email);
        }

        [Fact]
        public async Task CustomerOrderService_UnitTest_GetCustomerOrderAsync_CustomerNotFound()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });
            Mock<HttpClient> mockHttpClient = new Mock<HttpClient>(mockMessageHandler.Object);
            mockHttpClient.Object.BaseAddress = new Uri("https://customer-details.azurewebsites.net/api/GetUserDetails");
            var customerService = new CustomerService(mockHttpClient.Object);

            // Act
            var errorResponse = await Assert.ThrowsAsync<EShopOrdersException>(() => customerService.GetCustomerByEmailAsync("bob@mmtdigital.co.uk"));

            // Assert
            Assert.Equal(404, errorResponse.HttpStatusCode);
            Assert.Equal("The customer with email, bob@mmtdigital.co.uk is not found.", errorResponse.Message);

        }

        private CustomerDetail GetCustomerDetail()
        {
            return new CustomerDetail()
            {
                CustomerId = "R223232",
                Email = "bob@mmtdigital.co.uk",
                FirstName = "Bob",
                LastName = "Testperson",
                HouseNumber = "1a",
                Street = "Uppingham Gate",
                Town = "Uppingham",
                PostCode = "LE15 9NY"
            };

        }
    }
}
