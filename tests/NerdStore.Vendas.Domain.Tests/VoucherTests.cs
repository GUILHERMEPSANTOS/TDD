using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar se Voucher é valido")]
        [Trait("Categoria", "Voucher - Pedido")]
        public void Voucher_ValidarVoucherTipoValorFixo_DeveEstaeValido()
        {
            //Arrange
                var voucher = new Voucher(

                    codigo: "XPHCMT",
                    dataValidade: DateTime.UtcNow,
                    ativo: true,
                    quantidade:1,
                    tipoDesconto: TipoDesconto.ValorFixo,
                    valorDesconto: 1,
                    percentualDesconto: null
                );

            //Act
            var ehValido = voucher.ValidarSeEhValido();

            //Assert
            Assert.True(ehValido.IsValid);            
        }
    }
}
