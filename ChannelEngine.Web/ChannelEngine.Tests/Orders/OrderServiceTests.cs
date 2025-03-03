using ChannelEngine.Core.Models;
using ChannelEngine.Core.Services.Orders;
using FluentAssertions;
using Moq;

namespace ChannelEngine.Tests.Orders
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderApiClient> _apiClientMock;
        private readonly IOrderService _service;

        public OrderServiceTests()
        {
            _apiClientMock = new Mock<IOrderApiClient>();
            _service = new OrderService(_apiClientMock.Object);
        }

        [Fact]
        public async Task GetTop5ProductsAsync_ShouldReturnTop5Products()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Lines = new List<OrderLine> { new OrderLine { Gtin = "123", Description = "Product A", MerchantProductNo = "A1", Quantity = 10 } } },
                new Order { Lines = new List<OrderLine> { new OrderLine { Gtin = "123", Description = "Product A", MerchantProductNo = "A1", Quantity = 5 } } },
                new Order { Lines = new List<OrderLine> { new OrderLine { Gtin = "456", Description = "Product B", MerchantProductNo = "B1", Quantity = 20 } } },
                new Order { Lines = new List<OrderLine> { new OrderLine { Gtin = "789", Description = "Product C", MerchantProductNo = "C1", Quantity = 5 } } }
            };

            _apiClientMock.Setup(x => x.GetInProgressOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _service.GetTop5ProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(3);  // We expect 3 unique products here

            // Product B should be first with quantity 20
            result.First().Name.Should().Be("Product B");
            result.First().TotalQuantity.Should().Be(20);

            // Product C should be first with quantity 5
            result.Last().Name.Should().Be("Product C");
            result.Last().TotalQuantity.Should().Be(5);

            // Assert the order is correct
            result[0].TotalQuantity.Should().BeGreaterThanOrEqualTo(result[1].TotalQuantity);
            result[1].TotalQuantity.Should().BeGreaterThanOrEqualTo(result[2].TotalQuantity);
        }

        [Fact]
        public async Task GetTop5ProductsAsync_ShouldReturnEmptyList_WhenNoOrders()
        {
            // Arrange
            _apiClientMock.Setup(x => x.GetInProgressOrdersAsync()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _service.GetTop5ProductsAsync();

            // Assert
            result.Should().BeEmpty();
        }

    }
}
