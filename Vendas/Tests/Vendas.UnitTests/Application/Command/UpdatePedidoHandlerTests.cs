using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;
using Domain.Enums;
using Moq;

namespace Vendas.UnitTests.Application.Command;
public class UpdatePedidoHandlerTests
{
    [Fact]
    public async Task UpdatePedidoHandler_DeveAtualizarStatusDoPedido()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido(Guid.NewGuid(),
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m))
        ]);
        var mockRepository = new Mock<IPedidoRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()));
        var handler = new UpdatePedidoHandler(mockRepository.Object);
        var command = new UpdatePedido(pedidoId, PedidoStatus.Confirmado);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.Equal(PedidoStatus.Confirmado, pedido.Status);
        mockRepository.Verify(
            pr => pr.UpdateAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePedidoHandler_PedidoNaoEncontrado_DeveLancarExcecao()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var mockRepository = new Mock<IPedidoRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);
        var handler = new UpdatePedidoHandler(mockRepository.Object);
        var command = new UpdatePedido(pedidoId, PedidoStatus.Confirmado);
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
        mockRepository.Verify(
            pr => pr.UpdateAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
