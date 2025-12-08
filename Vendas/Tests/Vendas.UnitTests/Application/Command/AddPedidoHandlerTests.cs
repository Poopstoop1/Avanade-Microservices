using Application.Command;
using Application.Command.Handlers;
using Domain.Entities;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;

namespace Vendas.UnitTests.Application.Command;


public class AddPedidoHandlerTests
{
   [Fact]
   public async Task AddPedidoHandler_DeveAdicionarPedido()
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
        mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()));
       var handler = new AddPedidoHandler(mockRepository.Object);
        var command = new AddPedido(usuarioId,
        [
           new () {
                ProdutoId = itens[0].ProdutoId,
                NomeProduto = itens[0].NomeProduto,
                Quantidade = itens[0].Quantidade,
                PrecoUnitario = itens[0].PrecoUnitario
           }
        ]
       );
       // Act
       var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.NotEqual(Guid.Empty, result);
        mockRepository.Verify(
           pr => pr.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PedidoInvalido_NaoDeveChamarRepository()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var mockRepository = new Mock<IPedidoRepository>();
        var handler = new AddPedidoHandler(mockRepository.Object);
        var command = new AddPedido(usuarioId, []); // Lista vazia de itens
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(command, CancellationToken.None));
        mockRepository.Verify(
            pr => pr.AddAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}
