using Moq;
using Application.Queries;
using Application.Queries.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;

namespace Vendas.UnitTests.Application.Queries;


public class GetByIdPedidoHandlerTests
{
    [Fact]
    public async Task GetByIdPedidoHandler_Handle_DeveRetornarPedidoCorreto()
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
        var handler = new GetByIdPedidoHandler(mockRepository.Object);
        var query = new GetByIdPedido(pedido.Id);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedido.Id, result.Id);
        Assert.Equal(usuarioId, result.UsuarioId);
        Assert.Equal(2, result.Itens.Count);
        Assert.Equal(31, result.ValorTotal.Valor);
    }

    [Fact]
    public async Task PedidoInexistente_DeveLancarKeyNotFoundException()
    {
        // Arrange
        var mockRepository = new Mock<IPedidoRepository>();
        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Pedido?)null);
        var handler = new GetByIdPedidoHandler(mockRepository.Object);
        var query = new GetByIdPedido(Guid.NewGuid());
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        mockRepository.Verify(
           pr => pr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
