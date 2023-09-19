namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public int ValorUnitario { get; private set; }

        public PedidoItem(Guid id, Guid produtoId, string produtoNome, int quantidade, int valorUnitario)
        {
            Id = id;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        internal void AdicionarUnidade(int unidades)
        {
            Quantidade += unidades;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
