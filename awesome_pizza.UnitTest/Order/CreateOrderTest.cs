using AutoMapper;
using awesome_pizza.Application.Order;
using awesome_pizza.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awesome_pizza.UnitTest.Order
{
    public class CreateOrderTest
    {
        private readonly Mock<IOrderRepository> mockOrderRepository;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly CreateOrderHandler handler;

        public CreateOrderTest()
        {

            var mockMapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new CreateOrderProfile());
            });
            var mapper = mockMapper.CreateMapper();

            mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository.Setup(x => x.Create(It.IsAny<Domain.Entities.Order>())).Returns(new Domain.Entities.Order());

            mockUnitOfWork = new Mock<IUnitOfWork>();

            handler = new CreateOrderHandler(mapper, mockOrderRepository.Object, mockUnitOfWork.Object);
        }


        [Fact]
        public void CanCreateNewOrder_ReturnIdOfTheNewOrder()
        {
            // Arrange
            var request = new CreateOrderCommand(new CreateOrderRequest
            {
                Pizzas = new List<int> { 1, 2 }
            });

            // Act
            var result = handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.True(result.Id != 0);
        }


        [Fact]
        public async Task CanCreateNewOrderWith0Pizza_ThrowException()
        {
            // Arrange
            var request = new CreateOrderCommand(new CreateOrderRequest
            {
                Pizzas = new List<int>()
            });

            // Act / Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }

    }
}
