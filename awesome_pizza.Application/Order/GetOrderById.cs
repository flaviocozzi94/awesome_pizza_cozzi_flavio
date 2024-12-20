using awesome_pizza.Domain.Entities.Enumerators;
using awesome_pizza.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Application.Order
{
    public class GetOrderByIdQuery : IRequest<GetOrderByIdResponse>
    {
        public GetOrderByIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }

    public class GetOrderByIdResponse
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime InsertDate { get; set; }
    }

    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdResponse>
    {
        private IOrderRepository orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.OrderId);

            return new GetOrderByIdResponse
            {
                Id = order.Id,
                Status = order.Status,
                InsertDate = order.InsertDate
            };
        }
    }
}
