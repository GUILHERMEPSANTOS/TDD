using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Pedidos.Commands;
using NerdStore.Vendas.Application.Pedidos.Handlers;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {

        [Fact(DisplayName = "Adicionar item no pedido deve ser valido")]
        [Trait("Categoria", "PedidoCommandHandlerTests")]
        public async Task PedidoCommandHandler_NovoItem_DeveAdicionarComSucesso()
        {
            //Assert
            var itemCommand = new AdicionarItemCommand(
                   produtoId: Guid.NewGuid(),
                   clienteId: Guid.NewGuid(),
                   nome: "Livro",
                   quantidade: 2,
                   valorUnitario: 100
            );
            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();

            //Act
            var result = await pedidoCommandHandler.Handle(itemCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.Adicionar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
            ;
        }
    }
}
