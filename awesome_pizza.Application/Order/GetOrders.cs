using awesome_pizza.Domain.Entities.Enumerators;
using awesome_pizza.Domain.Repositories;
using MediatR;

namespace awesome_pizza.Application.Order
{
    public class GetOrdersQuery : IRequest<GetOrdersResponseWrapper>
    {
        public GetOrdersQuery(OrderStatus? orderStatus)
        {
            OrderStatus = orderStatus;
        }

        public OrderStatus? OrderStatus { get; }
    }

    public class GetOrdersResponseWrapper
    {
        public List<GetOrdersResponse> Data { get; set; }
        public class GetOrdersResponse
        {

            public Guid Id { get; set; }
            public OrderStatus OrderStatus { get; set; }
            public List<string> Pizzas { get; set; }
        }
    }

    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, GetOrdersResponseWrapper>
    {
        private readonly IOrderRepository orderRepository;

        public GetOrdersHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<GetOrdersResponseWrapper> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersAsync(request.OrderStatus);

            var result = new GetOrdersResponseWrapper
            {
                Data = new List<GetOrdersResponseWrapper.GetOrdersResponse>()
            };

            foreach (var o in orders)
            {
                var order = new GetOrdersResponseWrapper.GetOrdersResponse
                {
                    Id = o.Id,
                    OrderStatus = o.Status,
                    Pizzas = new List<string>()
                };
                foreach (var pizza in o.OrderPizzas.Select(x => x.Pizza))
                {
                    order.Pizzas.Add(pizza.Name);
                }
                result.Data.Add(order);
            }

            return result;
        }
    }
}
