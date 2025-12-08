using Xunit;
using Domain.Entities;
using Domain.IRepository;
using Moq;
using Application.Queries;
using Application.Queries.Handlers;

namespace Estoque.UnitTests.Application.Queries;

public class GetProductByIdTest
{
    [Fact]
    public async Task UmProduto_DeveRetornarUmProduto()
    {
        //Arrange
        var produto = new Produto("Produto 1", "Descricao 1", 10.0m, 100);
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(produto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto);
        var getProductByIdHandler = new GetProductByIdHandler(produtoRepositoryMock.Object);
        //Act
        var result = await getProductByIdHandler
            .Handle(new GetProductById(produto.Id), new CancellationToken());
        //Assert
        Assert.Equal(produto.Id, result.Id);
        Assert.NotNull(result);
        produtoRepositoryMock.Verify(
                pr => pr.GetByIdAsync(produto.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProdutoInexistente_DeveLancarExcecao()
    {
        //Arrange
        var produtoId = Guid.NewGuid();
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(produtoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);

        var getProductByIdHandler = new GetProductByIdHandler(produtoRepositoryMock.Object);
        //Act

        var excecao = await Assert.ThrowsAsync<KeyNotFoundException>
        (
            () => getProductByIdHandler.Handle(new GetProductById(produtoId), new CancellationToken())
        );

        //Assert
        Assert.Contains(produtoId.ToString(), excecao.Message);

        produtoRepositoryMock.Verify(
                pr => pr.GetByIdAsync(produtoId, It.IsAny<CancellationToken>()), Times.Once);
    }

}
