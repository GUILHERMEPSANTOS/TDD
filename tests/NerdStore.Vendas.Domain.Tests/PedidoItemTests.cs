using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo Item acima das unidades permitidas")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_ItensAcimaDasUnidadesPermitidas_DeveRetornarException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", Pedido.MAX_UNIDADES_PERMITIDAS + 1, 100));
        }

        [Fact(DisplayName = "Adicionar Item abaixo das unidades permitidas")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_ItensAbaixoDasUnidadesPermitidas_DeveRetornarException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", Pedido.MIX_UNIDADES_PERMITIDAS - 1, 100));
        }
    }
}
