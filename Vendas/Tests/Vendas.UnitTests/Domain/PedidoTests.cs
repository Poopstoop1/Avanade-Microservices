using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;


namespace Vendas.UnitTests.Domain;


public class PedidoTests
{
    [Fact]
    public void Pedido_CriarPedido_DeveCriarPedidoComItensEValorTotalCorreto()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        List<PedidoItem> itens =
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m)),
                new PedidoItem(Guid.NewGuid(), "Produto B", 1, new Preco(11.0m))
        ];

        // Act
        var pedido = new Pedido(usuarioId, itens);
        // Assert
        Assert.Equal(usuarioId, pedido.UsuarioId);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(31, pedido.ValorTotal.Valor);

    }

    [Fact]
    public void Pedido_PedidoConfirmado_DeveAlterarStatusParaConfirmado()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        List<PedidoItem> itens =
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m)),
                new PedidoItem(Guid.NewGuid(), "Produto B", 1, new Preco(11.0m))
        ];
        var pedido = new Pedido(usuarioId, itens);
        // Act
        pedido.PedidoConfirmado();
        // Assert
        Assert.Equal(PedidoStatus.Confirmado, pedido.Status);
    }

    [Fact]
    public void Pedido_PedidoCancelado_DeveAlterarStatusParaCancelado()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        List<PedidoItem> itens =
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m)),
                new PedidoItem(Guid.NewGuid(), "Produto B", 1, new Preco(11.0m))
        ];
        var pedido = new Pedido(usuarioId, itens);
        // Act
        pedido.PedidoCancelado();
        // Assert
        Assert.Equal(PedidoStatus.Cancelado, pedido.Status);
    }

    [Fact]
    public void Pedido_CriarPedido_SemItens_DeveLancarExcecao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        List<PedidoItem> itens = [];
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Pedido(usuarioId, itens));
    }

    [Fact]
    public void Pedido_UsuarioInvalido_DeveLancarExcecao()
    {
        // Arrange
        var invalidUsuarioId = Guid.Empty;
        List<PedidoItem> itens =
        [
            new PedidoItem(Guid.NewGuid(), "Produto A", 2, new Preco(10.0m))
        ];
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Pedido(invalidUsuarioId, itens));
    }

}
