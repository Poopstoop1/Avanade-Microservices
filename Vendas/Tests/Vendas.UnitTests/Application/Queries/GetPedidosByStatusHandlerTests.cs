using Application.Queries;
using Application.Queries.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;
using Domain.Enums;

namespace Vendas.UnitTests.Application.Queries;


public class GetPedidosByStatusHandlerTests
{
    [Fact]
    public async Task DoisPedidos_DeveRetornarPedidosComStatusEspecifico()
    {
        // Arrange
        var mockPedidoRepository = new Mock<IPedidoRepository>();
        List<Pedido> pedidos =
        [
            new 
            (
                usuarioId: Guid.NewGuid(),
                itens:
                [
                    new
                    (
                        produtoId: Guid.NewGuid(),
                        nomeProduto: "Produto A",
                        quantidade: 2,
                        new Preco( 20.0m)
                    )
                ]
            )
        ];
        mockPedidoRepository.Setup(repo => repo.GetByStatusAsync(It.IsAny<PedidoStatus>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidos);
        var handler = new GetByStatusPedidoHandler(mockPedidoRepository.Object);
        var request = new GetByStatusPedido(PedidoStatus.Criado);
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(pedidos[0].Id, result[0].Id);
        Assert.Equal(pedidos[0].Status, result[0].Status);
    }
    [Fact]
    public async Task NenhumPedido_DeveRetornarListaVazia()
    {
        // Arrange
        var mockPedidoRepository = new Mock<IPedidoRepository>();
        mockPedidoRepository.Setup(repo => repo.GetByStatusAsync(It.IsAny<PedidoStatus>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var handler = new GetByStatusPedidoHandler(mockPedidoRepository.Object);
        var request = new GetByStatusPedido(PedidoStatus.Confirmado);
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
