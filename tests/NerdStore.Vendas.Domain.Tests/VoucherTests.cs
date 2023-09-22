using NerdStore.Core.DomainObjects;
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
        public void ValidarVoucher_DataDeValidadeUtrapassada_DeveRetornarUmaException()
        {
            // Arrange
            var voucher = new Voucher("ABC", DateTime.UtcNow.AddDays(-1), true, 1, Desconto.ValorFixo);

            //Act & Assert
            Assert.Throws<DomainException>(() => voucher.ValidarSeEhValido());
        }
    }
}
