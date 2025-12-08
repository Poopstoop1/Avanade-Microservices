using Moq;
using Domain.IRepository;
using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;

namespace Estoque.UnitTests.Application.Command;

public class AddProductHandlerTests
{

    [Fact]
    public async Task ProdutoValido_DeveRetornarId()
    {
        //Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
       
        var addProductHandler = new AddProductHandler(produtoRepositoryMock.Object);
        var addProductCommand = new AddProduct("Produto 1", "Descricao 1", 10.0m, 100);
        //Act
        var result = await addProductHandler
            .Handle(addProductCommand, new CancellationToken());

        //Assert
        Assert.NotEqual(Guid.Empty, result);
        produtoRepositoryMock.Verify(
            pr => pr.AddAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProdutoInvalido_NaoDeveChamarRepository()
    {
        //Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();

        var handler = new AddProductHandler(produtoRepositoryMock.Object);

        var command = new AddProduct("", "", -10m, -100);

        //Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(command, CancellationToken.None)
        );

        produtoRepositoryMock.Verify(pr =>
            pr.AddAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}
