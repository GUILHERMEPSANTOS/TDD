using NerdStore.Core.Data;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
        void AdicionarItem(PedidoItem item);
        void Atualizar(Pedido pedido);
        Task<Pedido> ConsultarPedidoPorClientId(Guid clienteId);
        void AtualizarItem(PedidoItem pedidoItem);
    }
}
