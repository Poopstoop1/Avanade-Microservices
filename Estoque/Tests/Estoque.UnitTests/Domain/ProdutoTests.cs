using Domain.Entities;

namespace Estoque.UnitTests.Domain;


public class ProdutoTests
{
    private const string _nome = "Test Produto";
    private const string _descricao = "Testando Produto";
    private const decimal _preco = 99.99m;
    private const int _quantidade = 100;

    [Fact]
    public void CriarProduto_DeveRetornarParametrosValidos()
    {
        // Act
        var product = new Produto(_nome, _descricao, _preco, _quantidade);
        // Assert
        Assert.NotEmpty(product.Id.ToString());
        Assert.Equal(_nome, product.Nome);
        Assert.Equal(_descricao, product.Descricao);
        Assert.Equal(_preco, product.Preco.Valor);
        Assert.Equal(_quantidade, product.Quantidade);
    }

    
    [Fact]
    public void ConfirmarReserva_DeveDiminuirQuantidadeProduto()
    {
        // Arrange
        var produto = new Produto(_nome, _descricao, _preco, _quantidade);
        var diminuir = 10;
        produto.Reservar(diminuir);
        var expectedStock = _quantidade - diminuir;

        // Act
        produto.ConfirmarReserva(diminuir);

        // Assert
        Assert.Equal(expectedStock, produto.Quantidade);
        Assert.Equal(0, produto.QuantidadeReservada);
    }
}
