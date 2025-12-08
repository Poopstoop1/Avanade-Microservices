using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;


namespace Vendas.UnitTests.Application.Command;

public class DeleteProdutoHandlerTests
{
    [Fact]
    public async Task DeleteProdutoHandler_DeveDeletarProduto()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        List<PedidoItem> itens =
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m)),
           new PedidoItem(Guid.NewGuid(), "Produto B", 1, new Preco(11.0m))
        ];
        var pedido = new Pedido(usuarioId, itens);
        var mockRepository = new Mock<IPedidoRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);
        mockRepository.Setup(repo => repo.DeleteAsync(pedido, It.IsAny<CancellationToken>()));
        var handler = new DeletePedidoHandler(mockRepository.Object);
        var command = new DeletePedido(pedido.Id);
        // Act
        await handler.Handle(command, CancellationToken.None);
        // Assert
        mockRepository.Verify(
            pr => pr.DeleteAsync(pedido, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task DeleteProduto_ComIdInvalido_DeveLancarExcecao()
    {
        // Arrange
        var invalidPedidoId = Guid.NewGuid();
        var mockRepository = new Mock<IPedidoRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(invalidPedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);
        var handler = new DeletePedidoHandler(mockRepository.Object);
        var command = new DeletePedido(invalidPedidoId);
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
        mockRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}
