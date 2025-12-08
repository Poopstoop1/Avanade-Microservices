using Application.Queries;
using Application.Queries.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;

namespace Vendas.UnitTests.Application.Queries;


public class GetAllPedidosHandlerTests
{
    [Fact]
    public async Task DoisPedidos_DeveRetornarTodosPedidos()
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
            ),
            new
            (
                usuarioId: Guid.NewGuid(),
                itens:
                [
                    new 
                    (
                        produtoId: Guid.NewGuid(),
                        nomeProduto: "Produto B",
                        quantidade: 4,
                        new Preco( 50.0m)
                    )
                ]
            )
        ];

        mockPedidoRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedidos);
        var handler = new GetAllPedidoHandler(mockPedidoRepository.Object);
        var request = new GetByAllPedido();
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(pedidos[0].Id, result[0].Id);
        Assert.Equal(pedidos[1].Id, result[1].Id);

        mockPedidoRepository.Verify(
            pr => pr.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task NenhumPedido_DeveRetornarListaVazia()
    {
        // Arrange
        var mockPedidoRepository = new Mock<IPedidoRepository>();
        mockPedidoRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var handler = new GetAllPedidoHandler(mockPedidoRepository.Object);
        var request = new GetByAllPedido();
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        mockPedidoRepository.Verify(
           pr => pr.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}
