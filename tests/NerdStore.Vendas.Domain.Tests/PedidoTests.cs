using System.Reflection;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item ao Pedido")]
        [Trait("Categoria","Pedido Tests")]

        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 2, 100);

            //Act 
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(expected:200, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item existente ao Pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_ItemExistente_DeveAtualizarAQuantidadeDoItem()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);

            pedido.AdicionarItem(pedidoItem);

            //Act 
            pedido.AdicionarItem(pedidoItem2);

            //Assert
            Assert.Equal(expected: 300, actual: pedido.ValorTotal);
            Assert.Equal(expected: 2, actual: pedido.PedidoItems.Count);
        }
    }
}
