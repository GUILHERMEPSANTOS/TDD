using NerdStore.Core.Data;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
    }
}
