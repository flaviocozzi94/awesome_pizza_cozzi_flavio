using AutoMapper;
using awesome_pizza.Application.Order;
using awesome_pizza.Domain.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace awesome_pizza.Application.Pizza
{
    public class CreatePizzaCommand : IRequest<CreatePizzaResponse>
    {
        public CreatePizzaCommand(CreatePizzaRequest pizza)
        {
            Pizza = pizza;
        }

        public CreatePizzaRequest Pizza { get; }
    }

    public class CreatePizzaRequest
    {
        public required string Name { get; init; }
        public string? Description { get; set; }
        public required double Price { get; init; }
    }

    public class CreatePizzaResponse
    {
        public int PizzaId { get; set; }

    }

    public class CreatePizzaProfile : Profile
    {
        public CreatePizzaProfile()
        {
            CreateMap<CreatePizzaRequest, Domain.Entities.Pizza>()
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => true))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
        }
    }

    public class CreatePizzaHandler : IRequestHandler<CreatePizzaCommand, CreatePizzaResponse>
    {
        private readonly IPizzaRepository pizzaRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreatePizzaHandler(IPizzaRepository pizzaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.pizzaRepository = pizzaRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CreatePizzaResponse> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
        {
            var pizza = mapper.Map<Domain.Entities.Pizza>(request.Pizza);

            var pizzaCreated = pizzaRepository.Create(pizza);

            await unitOfWork.SaveChangesAsync();

            return new CreatePizzaResponse
            {
                PizzaId = pizzaCreated.Id
            };
        }
    }
}
