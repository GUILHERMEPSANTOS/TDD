using MediatR;
using NerdStore.Vendas.Application.Pedidos.Commands;
using NerdStore.Vendas.Application.Pedidos.Events;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Pedidos.Handlers
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemCommand message, CancellationToken cancellationToken)
        {
            var ehValido = message.ValidarSeEhValido();

            if (!ehValido) return false;

            var pedido = Pedido.PedidoFactory.GerarNovoPedido(message.ClienteId);
            var pedidoItem = new PedidoItem(
                produtoId: message.ProdutoId,
                produtoNome: message.Nome,
                quantidade: message.Quantidade,
                valorUnitario: message.ValorUnitario
             );

            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);

            await _mediator.Publish(new PedidoItemAdicionadoEvent(
                message.ClienteId,
                pedido.Id, 
                message.ProdutoId,
                message.Nome,
                message.Quantidade,
                message.ValorUnitario
            ), cancellationToken);

            return true;
        }
    }
}
