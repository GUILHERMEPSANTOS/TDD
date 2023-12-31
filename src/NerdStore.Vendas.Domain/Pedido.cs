﻿
using FluentValidation.Results;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity
    {
        public const int MAX_UNIDADES_PERMITIDAS = 15;
        public const int MIX_UNIDADES_PERMITIDAS = 1;
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal Desconto { get; set; }
        public PedidoStatus Status { get; private set; }
        public Voucher Voucher { get; private set; }
        public bool VoucherUtilizado { get; private set; }
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

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var ehValido = voucher.ValidarSeEhValido();

            if (!ehValido.IsValid)
            {
                return ehValido;
            }

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularDesconto();

            return ehValido;
        }

        public void RemoverItem(Guid produtoId)
        {
            ValidarSeItemEhInexistente(produtoId);

            var itemExistente = ObterItemPorProdutoId(produtoId);

            _pedidoItems.Remove(itemExistente);

            CalcularValorTotal();
        }

        public void AtualizarItem(int quantidade, Guid produtoId)
        {
            ValidarSeItemEhInexistente(produtoId);

            var item = ObterItemPorProdutoId(produtoId);
            item.AtualizarQuantidade(quantidade);

            CalcularValorTotal();
        }

        public void ValidarSeItemEhInexistente(Guid produtoId)
        {
            var possuiItem = VerificaSePossuiItem(produtoId);

            if (!possuiItem)
            {
                throw new DomainException("item não existe no pedido");
            }
        }

        public void AdicionarItem(PedidoItem novoItem)
        {
            ValidarQuantidadeDeItensPermitidas(novoItem.Quantidade, novoItem.ProdutoId);

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

        public void ValidarQuantidadeDeItensPermitidas(int quantidade, Guid produtoId)
        {
            var possuiItem = VerificaSePossuiItem(produtoId);

            if (possuiItem)
            {
                var itemExistente = ObterItemPorProdutoId(produtoId);
                quantidade += itemExistente.Quantidade;
            }

            if (quantidade > MAX_UNIDADES_PERMITIDAS) throw new DomainException($"O item ultrapassou a quantidade máxima permitida de {Pedido.MAX_UNIDADES_PERMITIDAS}");
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

            CalcularDesconto();
        }

        public void CalcularDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            if (Voucher.TipoDesconto == TipoDesconto.ValorFixo)
            {
                desconto = Voucher.ValorDesconto ?? 0;
            }
            else if (Voucher.TipoDesconto == TipoDesconto.Percentual)
            {
                if (Voucher.PercentualDesconto.HasValue)
                {
                    desconto = ValorTotal * (Voucher.PercentualDesconto.Value / 100);
                }
            }

            Desconto = desconto;
            ValorTotal = ValorTotal - Desconto < 0 ? 0 : ValorTotal - Desconto;
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
