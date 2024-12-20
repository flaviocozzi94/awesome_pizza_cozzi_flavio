using awesome_pizza.Application.Pizza;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace awesome_pizza_cozzi_flavio.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/pizzas")]
    public class PizzaController : ControllerBase
    {
        private readonly IMediator mediator;

        public PizzaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Create a new pizza
        /// </summary>
        /// <param name="pizza">the pizza to create</param>
        /// <returns>The id of the new pizza</returns>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreatePizzaAsync(CreatePizzaRequest pizza)
        {
            var version = RoutingHttpContextExtensions.GetRouteValue(HttpContext, "version");

            try
            {
                var result = await mediator.Send(new CreatePizzaCommand(pizza));
                return Created(Url.RouteUrl(nameof(GetPizzaByIdAsync)), new { pizzaId = result.PizzaId });
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while creating pizza.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Get the pizza by id
        /// </summary>
        /// <param name="pizzaId">The id of the pizza</param>
        /// <returns>The </returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{pizzaId:int}")]
        public async Task<IActionResult> GetPizzaByIdAsync([FromRoute] int pizzaId)
        {
            try
            {
                var result = await mediator.Send(new GetPizzaByIdQuery(pizzaId));
                return Ok(result);
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while getting pizza.",
                    statusCode: 500);
            }
        }

        /// <summary>
        /// Get all pizzas
        /// </summary>
        /// <param name="includeUnavailablePizza">to include unavailable pizzas</param>
        /// <returns>The list of pizza</returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllPizzaAsync([FromQuery] bool includeUnavailablePizza = false)
        {
            try
            {
                var result = await mediator.Send(new GetAllPizzaQuery(includeUnavailablePizza));
                return Ok(result);
            }
            catch (Exception)
            {
                return Problem(
                    detail: "Error while getting pizzas.",
                    statusCode: 500);
            }
        }
    }
}
