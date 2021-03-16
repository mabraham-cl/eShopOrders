using eShopOrders.API.Controllers;
using eShopOrders.Business;
using eShopOrders.Shared.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using eShopOrders.Shared;

namespace eShopOrders.UnitTests
{
    public class OrderController_UnitTests
    {
        [Fact]
        public async Task OrderController_UnitTests_GetMostRecentOrder_ReturnCustomerOrderAsync()
        {
            // Arrange
            var mockService = new Mock<ICustomerOrderService>();
            mockService.Setup(x => x.GetCustomerOrderAsync("cat.owner@mmtdigital.co.uk", "C34454")).ReturnsAsync(GetCustomerOrder());
            var mockIlogger = new Mock<ILogger<OrdersController>>();
            var controller = new OrdersController(mockIlogger.Object, mockService.Object);

            // Act
            var result = await controller.GetMostRecentOrder("cat.owner@mmtdigital.co.uk", "C34454");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<CustomerOrder>(
                okResult.Value);
            Assert.Equal(2, model.Order.OrderItems.Count);
            Assert.Equal("Charlie", model.Customer.FirstName);
            Assert.Equal("Cat", model.Customer.LastName);
            Assert.Equal("7-May-2021", model.Order.DeliveryExpected);
        }

        [Fact]
        public async Task OrderController_UnitTests_GetMostRecentOrder_CustomerNotFound()
        {
            // Arrange
            var mockService = new Mock<ICustomerOrderService>();
            mockService.Setup(x => x.GetCustomerOrderAsync("cat.owner@mmtdigital.co.uk", "C34454")).Throws(new EShopOrdersErrorResponse($"The customer with email, cat.owner@mmtdigital.co.uk is not found.", 404)); 
            var mockIlogger = new Mock<ILogger<OrdersController>>();
            var controller = new OrdersController(mockIlogger.Object, mockService.Object);

            // Act
            var result = await controller.GetMostRecentOrder("cat.owner@mmtdigital.co.uk", "C34454");

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            var model = Assert.IsAssignableFrom<EShopOrdersErrorResponse>(
                okResult.Value);
            Assert.Equal(404, model.HttpStatusCode);
            Assert.Equal("The customer with email, cat.owner@mmtdigital.co.uk is not found.", model.Message);
        }

        [Fact]
        public async Task OrderController_UnitTests_GetMostRecentOrder_InvalidCustomerEmail()
        {
            // Arrange
            var mockService = new Mock<ICustomerOrderService>();
            mockService.Setup(x => x.GetCustomerOrderAsync("", "C34454")).Throws(new EShopOrdersErrorResponse($"Please provide a valid email id.", 400));
            var mockIlogger = new Mock<ILogger<OrdersController>>();
            var controller = new OrdersController(mockIlogger.Object, mockService.Object);

            // Act
            var result = await controller.GetMostRecentOrder("", "C34454");

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = Assert.IsAssignableFrom<EShopOrdersErrorResponse>(
                okResult.Value);
            Assert.Equal(400, model.HttpStatusCode);
            Assert.Equal("Please provide a valid email id.", model.Message);
        }

        [Fact]
        public async Task OrderController_UnitTests_GetMostRecentOrder_InvalidCustomerId()
        {
            // Arrange
            var mockService = new Mock<ICustomerOrderService>();
            mockService.Setup(x => x.GetCustomerOrderAsync("cat.owner@mmtdigital.co.uk", "")).Throws(new EShopOrdersErrorResponse($"Please provide a valid customer id.", 400));
            var mockIlogger = new Mock<ILogger<OrdersController>>();
            var controller = new OrdersController(mockIlogger.Object, mockService.Object);

            // Act
            var result = await controller.GetMostRecentOrder("cat.owner@mmtdigital.co.uk", "");

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var model = Assert.IsAssignableFrom<EShopOrdersErrorResponse>(
                okResult.Value);
            Assert.Equal(400, model.HttpStatusCode);
            Assert.Equal("Please provide a valid customer id.", model.Message);
        }

        private CustomerOrder GetCustomerOrder()
        {
            return new CustomerOrder()
            {
                Customer = new CustomerName() { FirstName= "Charlie", LastName  = "Cat" },
                Order = new OrderDetail() {
                    DeliveryAddress = "1a, Uppingham Gate, Uppingham, LE15 9NY",
                    DeliveryExpected = "7-May-2021",
                    OrderDate = "11-Sep-2020",
                    OrderItems = new List<ProductDetail>() { 
                    new ProductDetail{PriceEach = 12.5M, Quantity=1, Product="'I love my pet' t-shirt" },
                    new ProductDetail{PriceEach =15.99M, Quantity=1, Product="'I love my pet' t-shirt" }
                    },
                    OrderNumber=4
                    }
            };

        }
    }
}
