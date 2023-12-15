using System.Data;
using AutoFixture;
using Dapper;
using EasyGroceries.Cart.Domain;
using EasyGroceries.Cart.Infrastructure.Contracts;
using EasyGroceries.Cart.Infrastructure.Repositories;
using Moq;

namespace EasyGroceries.Cart.Infrastructure.Tests;

public class CartHeaderRepositoryTests
{
    private readonly Mock<IDapper> _dapperMock;

    public CartHeaderRepositoryTests()
    {
        _dapperMock = new Mock<IDapper>();
    }

    [Fact]
    public void Add_Should_InsertsCartHeaderRecordInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var cartHeader = fixture.Create<CartHeader>();
        _dapperMock.Setup(x => x.Insert(It.IsAny<string>(), cartHeader, It.IsAny<CommandType>()))
                        .Returns(cartHeader);
        // Act
        CartHeaderRepository cartHeaderRepository = new CartHeaderRepository(_dapperMock.Object);
        var result = cartHeaderRepository.Add(cartHeader);

        // Assert
        _dapperMock.Verify(x => x.Insert(It.IsAny<string>(), cartHeader, It.IsAny<CommandType>()),
              Times.Once);
    }

    [Fact]
    public void Delete_Should_DeletesCartHeaderRecordFromDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var cartHeaders = fixture.Create<List<CartHeader>>();
        _dapperMock.Setup(x => x.Execute<CartHeader>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            cartHeaders.RemoveAt(1);
                        });

        var cartHeadersCountBeforeDelete = cartHeaders.Count();

        // Act
        CartHeaderRepository cartHeaderRepository = new CartHeaderRepository(_dapperMock.Object);
        var result = cartHeaderRepository.Delete(1);

        // Assert
        _dapperMock.Verify(x => x.Execute<CartHeader>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<CommandType>()),
              Times.Once);
        Assert.Equal(cartHeadersCountBeforeDelete - 1, cartHeaders.Count);
    }

    [Fact]
    public void GetCartHeaderByUserId_Should_Return_CartHeaderOfSpecifiedUserId()
    {
        // Arrange
        var fixture = new Fixture();
        var cartHeaderList = fixture.Create<IReadOnlyList<CartHeader>>();
        int cartHeaderIndex = 1;
        var expectedCartHeader = cartHeaderList[cartHeaderIndex];
        _dapperMock.Setup(x => x.Get<CartHeader>(It.IsAny<string>(),
                 It.IsAny<DynamicParameters>(), It.IsAny<CommandType>())).Returns(expectedCartHeader);

        // Act
        CartHeaderRepository cartHeaderRepository = new CartHeaderRepository(_dapperMock.Object);
        var result = cartHeaderRepository.GetCartHeaderByUserId(expectedCartHeader.UserId);

        // Assert
        Assert.Equal(expectedCartHeader.CartHeaderId, result.Result.CartHeaderId);
        Assert.Equal(expectedCartHeader.UserId, result.Result.UserId);
        Assert.Equal(expectedCartHeader.CartTotal, result.Result.CartTotal);
        Assert.Equal(expectedCartHeader.LoyaltyMembershipOpted, result.Result.LoyaltyMembershipOpted);
    }

}