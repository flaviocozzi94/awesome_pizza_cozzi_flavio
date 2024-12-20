using AutoMapper;
using awesome_pizza.Domain.Entities;
using awesome_pizza.Domain.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace awesome_pizza.Application.Order
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public CreateOrderCommand(CreateOrderRequest order)
        {
            Order = order;
        }

        public CreateOrderRequest Order { get; }
    }

    public class CreateOrderRequest
    {
        public List<int> Pizzas { get; set; }
    }

    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
    }

    public class CreateOrderProfile : Profile
    {
        public CreateOrderProfile()
        {
            CreateMap<CreateOrderRequest, Domain.Entities.Order>()
                .ForMember(d => d.Id, o => o.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.Status, o => o.MapFrom(s => Domain.Entities.Enumerators.OrderStatus.Open))
                .ForMember(d => d.OrderPizzas, o => o.MapFrom(s => s.Pizzas.Select(x => new OrderPizza { PizzaId = x }).ToList()))
                .ForMember(d => d.IsDeleted, o => o.MapFrom(s => false))
                .ForMember(d => d.InsertDate, o => o.MapFrom(s => DateTime.UtcNow));

        }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrderHandler(IMapper mapper, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (!CanCreateOrder(request))
                throw new Exception("The order must have at least 1 pizza");

            Domain.Entities.Order result = CreateOrderAsync(request);

            await unitOfWork.SaveChangesAsync();
            return new CreateOrderResponse
            {
                OrderId = result.Id
            };
        }

        private bool CanCreateOrder(CreateOrderCommand request)
        {
            if (request.Order.Pizzas.Count == 0)
                return false;

            return true;
        }

        private Domain.Entities.Order CreateOrderAsync(CreateOrderCommand request)
        {
            var order = mapper.Map<CreateOrderRequest, Domain.Entities.Order>(request.Order);
            var result = orderRepository.Create(order);
            return result;
        }
    }
}
