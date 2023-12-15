using System.Data;
using AutoFixture;
using Dapper;
using EasyGroceries.Cart.Domain;
using EasyGroceries.Cart.Infrastructure.Contracts;
using EasyGroceries.Cart.Infrastructure.Repositories;
using Moq;

namespace EasyGroceries.Cart.Infrastructure.Tests;

public class CartDetailsRepositoryTests
{
    private readonly Mock<IDapper> _dapperMock;

    public CartDetailsRepositoryTests()
    {
        _dapperMock = new Mock<IDapper>();
    }

    [Fact]
    public void AddCartDetails_Should_InsertsCartDetailsRecordInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var cartDetails = fixture.Create<CartDetails>();
        List<CartDetails> cartDetailsLst = new List<CartDetails>();
        _dapperMock.Setup(x => x.Insert(It.IsAny<string>(), cartDetails, It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            cartDetailsLst.Add(cartDetails);
                        });
        // Act
        CartDetailsRepository cartDetailsRepository = new CartDetailsRepository(_dapperMock.Object);
        var result = cartDetailsRepository.AddCartDetails(cartDetails);

        // Assert
        _dapperMock.Verify(x => x.Insert(It.IsAny<string>(), cartDetails, It.IsAny<CommandType>()),
              Times.Once);
        Assert.Equal(1, cartDetailsLst.Count);
        Assert.Equal(cartDetails.CartHeaderId, cartDetailsLst.FirstOrDefault().CartHeaderId);
        Assert.Equal(cartDetails.CartDetailsId, cartDetailsLst.FirstOrDefault().CartDetailsId);
        Assert.Equal(cartDetails.Count, cartDetailsLst.FirstOrDefault().Count);
        Assert.Equal(cartDetails.ProductId, cartDetailsLst.FirstOrDefault().ProductId);
    }

    [Fact]
    public void GetAllCartDetails_Should_ReturnAllCartDetailsRecordPresentInDB()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var cartDetailsLst = fixture.Create<List<CartDetails>>();
        _dapperMock.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<CartDetails>(), It.IsAny<CommandType>()))
                        .Returns(cartDetailsLst);
        // Act
        CartDetailsRepository cartDetailsRepository = new CartDetailsRepository(_dapperMock.Object);
        var result = cartDetailsRepository.GetAllCartDetails();

        // Assert
        Assert.Equal(cartDetailsLst.Count, result.Result.Count);
        Assert.Equal(cartDetailsLst.FirstOrDefault().CartHeaderId, result.Result.FirstOrDefault().CartHeaderId);
        Assert.Equal(cartDetailsLst.FirstOrDefault().CartDetailsId, result.Result.FirstOrDefault().CartDetailsId);
        Assert.Equal(cartDetailsLst.FirstOrDefault().Count, result.Result.FirstOrDefault().Count);
        Assert.Equal(cartDetailsLst.FirstOrDefault().ProductId, result.Result.FirstOrDefault().ProductId);
    }

    [Fact]
    public void UpdateCount_Should_InsertsCartDetailsRecordInDBSuccessfully()
    {
        // Arrange
        Fixture fixture = new Fixture();
        var cartDetails = fixture.Create<CartDetails>();
        List<CartDetails> cartDetailsLst = new List<CartDetails>();
        cartDetailsLst.Add(cartDetails);
        _dapperMock.Setup(x => x.Update<CartDetails>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<CommandType>()))
                        .Callback(() =>
                        {
                            cartDetailsLst[0].Count = cartDetailsLst[0].Count + 10;
                        });

        var countBeforeUpdate = cartDetailsLst[0].Count;
        // Act
        CartDetailsRepository cartDetailsRepository = new CartDetailsRepository(_dapperMock.Object);
        var result = cartDetailsRepository.UpdateCount(cartDetails);

        // Assert
        _dapperMock.Verify(x => x.Update<CartDetails>(It.IsAny<string>(), It.IsAny<DynamicParameters>(), It.IsAny<CommandType>()),
              Times.Once);
        Assert.NotEqual(countBeforeUpdate, cartDetailsLst[0].Count);
        Assert.Equal(countBeforeUpdate + 10, cartDetailsLst[0].Count);
    }

}