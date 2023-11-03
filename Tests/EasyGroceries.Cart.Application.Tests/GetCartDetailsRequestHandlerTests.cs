


using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.Features.CartDetails.Handlers.Queries;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries;
using EasyGroceries.Cart.Application.Profiles;
using EasyGroceries.Cart.Domain;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class GetCartDetailsRequestHandlerTests
{
    private readonly Mock<ICartDetailsRepository> _cartDetailsRepositoryMock;
    private List<CartDetails> _cartDetailsLst;

    public GetCartDetailsRequestHandlerTests()
    {
        _cartDetailsRepositoryMock = new Mock<ICartDetailsRepository>();
    }

    private void IntializeCartDetailsData()
    {
        _cartDetailsLst = new List<CartDetails>()
        {
            new CartDetails(){CartDetailsId = 1001, CartHeaderId = 1, Count = 4, ProductId = 25},
            new CartDetails(){CartDetailsId = 2001, CartHeaderId = 2, Count = 9, ProductId = 21},
            new CartDetails(){CartDetailsId = 3001, CartHeaderId = 3, Count = 5, ProductId = 15},
            new CartDetails(){CartDetailsId = 4000, CartHeaderId = 4, Count = 12, ProductId = 35},
            new CartDetails(){CartDetailsId = 5055, CartHeaderId = 5, Count = 3, ProductId = 18},
        };
    }

    [Fact]
    public async Task Handle_Should_ReturnsCartDetailsList()
    {
        // Arrange
        IntializeCartDetailsData();
        var command = new GetCartDetailsRequest();

        _cartDetailsRepositoryMock.Setup(x => x.GetAllCartDetails())
                .ReturnsAsync(_cartDetailsLst);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new GetCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Count.Equals(5);
    }


}