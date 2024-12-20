using awesome_pizza.Domain.Repositories;
using MediatR;

namespace awesome_pizza.Application.Pizza
{
    public class GetPizzaByIdQuery : IRequest<GetPizzaByIdResponse>
    {
        public GetPizzaByIdQuery(int pizzaId)
        {
            PizzaId = pizzaId;
        }

        public int PizzaId { get; }
    }

    public class GetPizzaByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class GetPizzaByIdHandler : IRequestHandler<GetPizzaByIdQuery, GetPizzaByIdResponse>
    {
        private IPizzaRepository pizzaRepository;

        public GetPizzaByIdHandler(IPizzaRepository pizzaRepository)
        {
            this.pizzaRepository = pizzaRepository;
        }

        public async Task<GetPizzaByIdResponse> Handle(GetPizzaByIdQuery request, CancellationToken cancellationToken)
        {
            var pizza = await pizzaRepository.GetPizzaByIdAsync(request.PizzaId);

            return new GetPizzaByIdResponse
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Description = pizza.Description,
                Price = pizza.Price,
                IsAvailable = pizza.IsAvailable,
            };
        }
    }
}
