using eShopOrders.Business;
using eShopOrders.Shared;
using eShopOrders.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eShopOrders.API.Controllers
{
    /// <summary>
    /// API that retrieves the details about delivery of most recent order for a customer.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger object</param>
        /// <param name="customerOrderService">Interface object of CustomerService</param>
        public OrdersController(ILogger<OrdersController> logger, ICustomerOrderService customerOrderService )
        {
            _logger = logger;
            _customerOrderService = customerOrderService;
        }

        /// <summary>
        /// Retrieves the details about the most recent order for the customer
        /// </summary>
        /// <param name="user">Email id of customer</param>
        /// <param name="customerId">Unique identifier of the customer</param>
        /// <returns>Most recent customer order details</returns>
        /// <response code="200">Success</response>       
        /// <response code="400">If the provided email or customerid isn't valid</response>
        /// <response code="404">Customer is not found</response>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerOrder), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMostRecentOrder([FromQuery] string user, [FromQuery] string customerId)
        {
            try
            {
                _logger.LogInformation($"Request received for GetMostRecentOrder for customer with email {user} and customerId {customerId}.", user, customerId);
                return Ok(await _customerOrderService.GetCustomerOrderAsync(user, customerId));
            }
            catch (EShopOrdersException exception)
            {
                _logger.LogError($"The following error occured on handling the request for customer with email {user} and customerId {customerId}. Error Message, {exception.Message}. StackTrace {exception.StackTrace}. HttpStatusCode {exception.HttpStatusCode}", user, customerId);

                if (exception.HttpStatusCode == 400)
                    return new BadRequestObjectResult(new EShopOrdersErrorResponse() { HttpStatus = exception.HttpStatusCode, Message = exception.Message }); 
                else if (exception.HttpStatusCode == 404)
                    return new NotFoundObjectResult(new EShopOrdersErrorResponse() { HttpStatus = exception.HttpStatusCode, Message = exception.Message });
                else
                    return new ObjectResult(new EShopOrdersErrorResponse() { HttpStatus = exception.HttpStatusCode, Message = exception.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"The following error occured on handling the request for customer with email {user} and customerId {customerId}. {ex.Message}.", user, customerId);
                return new ObjectResult(new EShopOrdersErrorResponse() { HttpStatus =500, Message = $"Internal Server Error! {ex.Message}", DeveloperMessage = ex.StackTrace }); 
            }
        }

        /// <summary>
        /// Logger object
        /// </summary>
        private readonly ILogger<OrdersController> _logger;

        /// <summary>
        /// Interface object of CustomerService
        /// </summary>
        private readonly ICustomerOrderService _customerOrderService;
    }
}
