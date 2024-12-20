using awesome_pizza.Application.Order;
using awesome_pizza.Domain.Entities.Enumerators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace awesome_pizza_cozzi_flavio.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get the order by id
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <returns>The order</returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] Guid orderId)
        {
            try
            {

                var result = await mediator.Send(new GetOrderByIdQuery(orderId));
                return Ok(result);
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while getting order.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Get the list of orders
        /// </summary>
        /// <param name="orderStatus">The status of the order</param>
        /// <returns>The list of orders</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] OrderStatus? orderStatus)
        {
            try
            {

                var result = await mediator.Send(new GetOrdersQuery(orderStatus));
                return Ok(result);
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while getting order.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="order">the order to create</param>
        /// <returns>The id of the new order</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderRequest order)
        {
            try
            {
                var result = await mediator.Send(new CreateOrderCommand(order));
                return Created(Url.RouteUrl(nameof(GetOrderByIdAsync)), new { orderId = result.OrderId });
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while creating order.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Patch the status of the order
        /// </summary>
        /// <param name="order">The object with the new status</param>
        /// <param name="orderId">The id of the order</param>
        /// <returns>The id of the order</returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPatch("{orderId:guid}")]
        public async Task<IActionResult> PatchOrderAsync(PatchStatusOrderRequest order, [FromRoute] Guid orderId)
        {
            try
            {

                order.Id = orderId;
                var result = await mediator.Send(new PatchStatusOrderCommand(order));
                return Ok(result);
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while patching order.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Delete the order
        /// </summary>
        /// <param name="orderId">the id of the order</param>
        /// <returns>Nothing</returns>
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{orderId:guid}")]
        public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid orderId)
        {
            try
            {

                var result = await mediator.Send(new DeleteOrderCommand(orderId));
                return NoContent();
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while deleting order.",
                    statusCode: 500);
                throw;
            }
        }
    }
}
