namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus Status { get; private set; }
        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public void AdicionarItem(PedidoItem novoItem)
        {
            var possuiItem = VerificaSePossuiItem(novoItem.ProdutoId);

            if (possuiItem)
            {
                var pedidoItemExistente = ObterItemPorProdutoId(novoItem.ProdutoId);
                pedidoItemExistente.AdicionarUnidade(novoItem.Quantidade);

                novoItem = pedidoItemExistente;
                _pedidoItems.Remove(pedidoItemExistente);
            }

            _pedidoItems.Add(novoItem);
            CalcularValorTotal();
        }

        public PedidoItem ObterItemPorProdutoId(Guid produtoId)
        {
            return _pedidoItems.FirstOrDefault(item => item.ProdutoId == produtoId);
        }
        public bool VerificaSePossuiItem(Guid produtoId)
        {
            return _pedidoItems.Any(item => item.ProdutoId == produtoId);
        }

        public void CalcularValorTotal()
        {
            ValorTotal = _pedidoItems.Sum(pedidoItem => pedidoItem.CalcularValor());
        }

        public static class PedidoFactory
        {
            public static Pedido GerarNovoPedido(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}
