using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ValidarSeQuantidadeEhValida(quantidade);

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        internal void AdicionarUnidade(int unidades)
        {
            Quantidade += unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AtualizarQuantidade(int quantidade)
        {
            ValidarSeQuantidadeEhValida(quantidade);
            Quantidade = quantidade;
        }

        internal void ValidarSeQuantidadeEhValida(int quantidade)
        {
            if (quantidade > Pedido.MAX_UNIDADES_PERMITIDAS) throw new DomainException($"O item ultrapassou a quantidade máxima permitida de {Pedido.MAX_UNIDADES_PERMITIDAS}");
            if (quantidade < Pedido.MIX_UNIDADES_PERMITIDAS) throw new DomainException($"Quantidade de itens abaixo da permitida que é {Pedido.MAX_UNIDADES_PERMITIDAS}");
        }
    }
}
