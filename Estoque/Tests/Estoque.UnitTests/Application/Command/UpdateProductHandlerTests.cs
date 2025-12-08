using Moq;
using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;
using Domain.IRepository;


namespace Estoque.UnitTests.Application.Command;

public class UpdateProductHandlerTests
{
    [Fact]
    public async Task ProdutoExistente_DeveChamarUpdateAsync()
    {
        //Arrange
        var produto = new Produto("Produto 1", "Descricao 1", 10.0m, 100);
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(produto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto);
        produtoRepositoryMock
        .Setup(pr => pr.Update(It.IsAny<Produto>(), It.IsAny<CancellationToken>()));
        var updateProductHandler = new UpdateProductHandler(produtoRepositoryMock.Object);
        var updateProductCommand = new UpdateProduct(produto.Id, "Produto Atualizado", "Descricao Atualizada", 20.0m, 200);
        //Act
        await updateProductHandler
            .Handle(updateProductCommand, new CancellationToken());
        //Assert
        Assert.Equal("Descricao Atualizada", produto.Descricao);
        Assert.Equal("Produto Atualizado", produto.Nome);
        Assert.Equal(20.0m, produto.Preco.Valor);
        Assert.Equal(200, produto.Quantidade);
        produtoRepositoryMock.Verify(
            pr => pr.Update(produto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProdutoInexistente_DeveLancarKeyNotFoundException()
    {
        //Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock
            .Setup(pr => pr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);
        var updateProductHandler = new UpdateProductHandler(produtoRepositoryMock.Object);
        var updateProductCommand = new UpdateProduct(Guid.NewGuid(), "Produto Atualizado", "Descricao Atualizada", 20.0m, 200);
        //Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await updateProductHandler.Handle(updateProductCommand, new CancellationToken()));
        produtoRepositoryMock.Verify( 
            pr => pr.Update(It.IsAny<Produto>(), It.IsAny<CancellationToken>()),Times.Never);

    }
}
