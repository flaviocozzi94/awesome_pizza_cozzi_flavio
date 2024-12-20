using awesome_pizza.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Application.Pizza
{

    public class GetAllPizzaQuery : IRequest<GetAllPizzaResponseWrapper>
    {
        public GetAllPizzaQuery(bool includeUnavailablePizza)
        {
            IncludeUnavailablePizza = includeUnavailablePizza;
        }

        public bool IncludeUnavailablePizza { get; }
    }

    public class GetAllPizzaResponseWrapper
    {
        public List<GetAllPizzaResponse> Data { get; set; }
        public class GetAllPizzaResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public double Price { get; set; }

        }
    }

    public class GetAllPizzaHandler : IRequestHandler<GetAllPizzaQuery, GetAllPizzaResponseWrapper>
    {
        private readonly IPizzaRepository pizzaRepository;

        public GetAllPizzaHandler(IPizzaRepository pizzaRepository)
        {
            this.pizzaRepository = pizzaRepository;
        }

        public async Task<GetAllPizzaResponseWrapper> Handle(GetAllPizzaQuery request, CancellationToken cancellationToken)
        {
            var pizzas = await pizzaRepository.GetAllPizzaAsync(request.IncludeUnavailablePizza);

            var result = new GetAllPizzaResponseWrapper
            {
                Data = new List<GetAllPizzaResponseWrapper.GetAllPizzaResponse>()
            };

            foreach (var pizza in pizzas)
            {
                result.Data.Add(new GetAllPizzaResponseWrapper.GetAllPizzaResponse
                {
                    Description = pizza.Description,
                    Id = pizza.Id,
                    Name = pizza.Name,
                    Price = pizza.Price
                });
            }

            return result;
        }
    }
}
