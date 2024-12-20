using AutoMapper;
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
    public class PatchStatusOrderCommand : IRequest<PatchStatusOrderResponse>
    {
        public PatchStatusOrderCommand(PatchStatusOrderRequest order)
        {
            Order = order;
        }

        public PatchStatusOrderRequest Order { get; }
    }

    public class PatchStatusOrderRequest
    {
        public OrderStatus OrderStatus { get; set; }
        public Guid Id { get; set; }
    }

    public class PatchStatusOrderResponse
    {
        public Guid Id { get; set; }
    }

    public class PatchStatusOrderHandler : IRequestHandler<PatchStatusOrderCommand, PatchStatusOrderResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public PatchStatusOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<PatchStatusOrderResponse> Handle(PatchStatusOrderCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Order order = await UpdateOrderDataAsync(request);

            await unitOfWork.SaveChangesAsync();

            return new PatchStatusOrderResponse
            {
                Id = order.Id
            };
        }

        private async Task<Domain.Entities.Order> UpdateOrderDataAsync(PatchStatusOrderCommand request)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.Order.Id);
            order.Status = request.Order.OrderStatus;
            return order;
        }
    }
}
