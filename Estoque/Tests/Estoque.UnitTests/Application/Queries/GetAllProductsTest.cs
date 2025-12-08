using Domain.Entities;
using Domain.IRepository;
using Moq;
using Application.Queries.Handlers;
using Application.Queries;
namespace Estoque.UnitTests.Application.Queries;

public class GetAllProductsTest
{

    [Fact]
    public async Task TresProdutos_DeveRetornarTresProdutos()
    {
        //Arrange
        List<Produto> produtos = [
            new Produto("Produto 1", "Descricao 1", 10.0m, 100),
            new Produto("Produto 2", "Descricao 2", 20.0m, 200),
            new Produto("Produto 3", "Descricao 3", 30.0m, 300)
            ];
        
        var produtoRepositoryMock = new Mock<IProdutoRepository>();

        produtoRepositoryMock
            .Setup(pr => pr.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        var getAllProductsHandler = new GetAllProductsHandler(produtoRepositoryMock.Object);

        //Act
        var result = await getAllProductsHandler
            .Handle(new GetAllProducts(), new CancellationToken());

        //Assert
        Assert.Equal(produtos.Count, result.Count);
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        produtoRepositoryMock.Verify(
            pr => pr.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task NenhumProduto_DeveRetornarListaVazia()
    {
        //Arrange
        List<Produto> produtos = [];
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);
        var getAllProductsHandler = new GetAllProductsHandler(produtoRepositoryMock.Object);
        //Act
        var result = await getAllProductsHandler
            .Handle(new GetAllProducts(), new CancellationToken());
        //Assert
        Assert.Empty(result);
        Assert.NotNull(result);
        produtoRepositoryMock.Verify(
            pr => pr.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
