using Domain.Entities;
using Domain.ValueObjects;

namespace Vendas.UnitTests.Domain;


public class PedidoItemTests
{
    [Fact]
    public void PedidoItem_CriarPedidoItem_DeveCriarPedidoItemComPropriedadesCorretas()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var nomeProduto = "Produto Teste";
        var quantidade = 3;
        var preco = new Preco(15.0m);
        // Act
        var pedidoItem = new PedidoItem(produtoId, nomeProduto, quantidade, preco);
        // Assert
        Assert.Equal(produtoId, pedidoItem.ProdutoId);
        Assert.Equal(nomeProduto, pedidoItem.NomeProduto);
        Assert.Equal(quantidade, pedidoItem.Quantidade);
        Assert.Equal(preco.Valor, pedidoItem.PrecoUnitario);
        Assert.Equal(45.0m, pedidoItem.Subtotal);
    }

    [Fact]
    public void PedidoItem_Subtotal_DeveCalcularSubtotalCorretamente()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var nomeProduto = "Produto Teste";
        var quantidade = 4;
        var preco = new Preco(12.5m);
        var pedidoItem = new PedidoItem(produtoId, nomeProduto, quantidade, preco);
        // Act
        var subtotal = pedidoItem.Subtotal;
        // Assert
        Assert.Equal(50.0m, subtotal);
    }

    [Fact]
    public void PedidoItem_CriarPedidoItem_ComQuantidadeInvalida_DeveLancarExcecao()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var nomeProduto = "Produto Teste";
        var quantidade = 0;
        var preco = new Preco(10.0m);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PedidoItem(produtoId, nomeProduto, quantidade, preco));
    }

    [Fact]
    public void PedidoItem_CriarPedidoItem_ComPrecoNegativo_DeveLancarExcecao()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var nomeProduto = "Produto Teste";
        var quantidade = 2;
        var preco = new Preco(-5.0m);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PedidoItem(produtoId, nomeProduto, quantidade, preco));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void PedidoItem_CriarPedidoItem_ComNomeProdutoInvalido_DeveLancarExcecao(string nomeProduto)
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var quantidade = 2;
        var preco = new Preco(10.0m);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PedidoItem(produtoId, nomeProduto, quantidade, preco));
    }

    [Fact]
    public void PedidoItem_CriarPedidoItem_ComIdVazio_DeveLancarExcecao()
    {
        // Arrange
        var produtoId = Guid.Empty;
        var nomeProduto = "Produto Teste";
        var quantidade = 2;
        var preco = new Preco(10.0m);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PedidoItem(produtoId, nomeProduto, quantidade, preco));
    }
}
