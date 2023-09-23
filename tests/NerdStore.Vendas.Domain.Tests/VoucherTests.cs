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
          
        }
    }
}
