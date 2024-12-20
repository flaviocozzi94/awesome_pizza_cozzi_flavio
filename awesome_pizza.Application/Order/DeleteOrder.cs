using awesome_pizza.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.Application.Order
{
    public class DeleteOrderCommand : IRequest<DeleteOrderResponse>
    {
        public DeleteOrderCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class DeleteOrderResponse
    {
        public Guid Id { get; set; }
    }

    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<DeleteOrderResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsync(request.Id);

            order.IsDeleted = true;

            await unitOfWork.SaveChangesAsync();

            return new DeleteOrderResponse { Id = request.Id };
        }
    }
}
