using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Moq;

namespace Estoque.UnitTests.Application.Command;

public class DeleteProductTest
{

    [Fact]
    public async Task ProdutoExistente_DeveChamarDeleteAsync()
    {
        //Arrange
        var produto = new Produto("Produto 1", "Descricao 1", 10.0m, 100);
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(produto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto);

        produtoRepositoryMock
        .Setup(pr => pr.Delete(It.IsAny<Produto>(), It.IsAny<CancellationToken>()));

        var deleteProductHandler = new DeleteProductHandler(produtoRepositoryMock.Object);
        var deleteProductCommand = new DeleteProduct(produto.Id);
        //Act
        await deleteProductHandler
            .Handle(deleteProductCommand, new CancellationToken());
        //Assert
        produtoRepositoryMock.Verify(
            pr => pr.Delete(produto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProdutoInexistente_DeveLancarKeyNotFoundException()
    {
        //Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);
        var deleteProductHandler = new DeleteProductHandler(produtoRepositoryMock.Object);
        var deleteProductCommand = new DeleteProduct(Guid.NewGuid());
        //Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await deleteProductHandler.Handle(deleteProductCommand, new CancellationToken()));
    }
}

