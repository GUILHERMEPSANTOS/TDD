using NerdStore.Core.DomainObjects;
using System.Reflection;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item ao Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]

        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 2, 100);

            //Act 
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(expected: 200, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item existente ao Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
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

        [Fact(DisplayName = "Adicionar Item existente ao Pedido acima das unidades Permitidas")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistentAcimaDasUnidadesPermitidas_DeveLancarUmaException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", Pedido.MAX_UNIDADES_PERMITIDAS, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 1, 100);

            pedido.AdicionarItem(pedidoItem);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }


        [Fact(DisplayName = "Tentar Atualizar Item não existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExistenteParaAtualizacao_DeveLancarUmaException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(10, produtoId));
        }

        [Fact(DisplayName = "Atualizar item do pedido acrescentando unidades")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComMaisUnidades_DeveAcrescentarAsUnidadesDoItem()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act
            pedido.AtualizarItem(5, produtoId);

            // Assert
            Assert.Equal(expected: 5, actual: pedidoItem.Quantidade);
        }

        [Fact(DisplayName = "Atualizar item do pedido decrementando unidades")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComMenosUnidades_DeveDecrementarAsUnidadesDoItem()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act
            pedido.AtualizarItem(2, produtoId);

            // Assert
            Assert.Equal(expected: 2, actual: pedidoItem.Quantidade);
        }

        [Fact(DisplayName = "Atualizar item do pedido decrementando unidades alem do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComMenosUnidadesQuePermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(Pedido.MIX_UNIDADES_PERMITIDAS - 1, produtoId));
        }

        [Fact(DisplayName = "Atualizar item do pedido adicionando unidades alem do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemComMaisUnidadesQuePermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(Pedido.MAX_UNIDADES_PERMITIDAS + 1, produtoId));
        }

        [Fact(DisplayName = "Atualizar item do pedido deve calcular valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ValorTotalAlterado_DeveRetornarValorTotalPedidoAtualizado()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act
            pedido.AtualizarItem(1, produtoId);

            //Assert
            Assert.Equal(expected: 100, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Remover item do pedido inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemInexistente_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(produtoId));
        }

        [Fact(DisplayName = "Remover item do pedido existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveRemoverItemDoPedido()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);

            //Act
            pedido.RemoverItem(produtoId);

            //Assert
            Assert.Empty(pedido.PedidoItems);
        }

        [Fact(DisplayName = "Remover item do pedido existente deve calcular valor pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveRecalcularValorTatol()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), produtoId, "Livro", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 3, 100);
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);

            //Act
            pedido.RemoverItem(produtoId);

            //Assert
            Assert.Equal(expected: 300, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher ao pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.ValorFixo,
               valorDesconto: 1,
               percentualDesconto: null
           );

            //Act
            var resultado = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(resultado.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher invalido ao pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.ValorFixo,
               valorDesconto: null,
               percentualDesconto: null
           );

            //Act
            var resultado = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.False(resultado.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher valido deve alterar valor total pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValorFixo_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.ValorFixo,
               valorDesconto: 10,
               percentualDesconto: null
           );

            pedido.AdicionarItem(pedidoItem);

            //Act
            var resultado = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(resultado.IsValid);
            Assert.Equal(expected: 90, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher de desconto percentual deve alterar valor total pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherPercentual_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.Percentual,
               valorDesconto: null,
               percentualDesconto: 50
           );

            pedido.AdicionarItem(pedidoItem);

            //Act
            var resultado = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(resultado.IsValid);
            Assert.Equal(expected: 50, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher com desconto maior que o valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValorFixo_DeveAtualizarValorTotalPara0()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.ValorFixo,
               valorDesconto: 1000,
               percentualDesconto: null
           );

            pedido.AdicionarItem(pedidoItem);

            //Act
            var resultado = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(resultado.IsValid);
            Assert.Equal(expected: 0, actual: pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher com desconto Modificar pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_ModificarPedido_DeveAtualizarValorTotalComDesconto()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), Guid.NewGuid(), "Livro", 1, 100);
            var voucher = new Voucher(
               codigo: "XPHCMT",
               dataValidade: DateTime.UtcNow,
               ativo: true,
               quantidade: 1,
               tipoDesconto: TipoDesconto.Percentual,
               valorDesconto: null,
               percentualDesconto: 50
           );

            pedido.AdicionarItem(pedidoItem);
            var resultado = pedido.AplicarVoucher(voucher);

            //Act
            pedido.AdicionarItem(pedidoItem2);
            //Assert
            Assert.True(resultado.IsValid);
            Assert.Equal(expected: 100, actual: pedido.ValorTotal);
        }
    }
}
