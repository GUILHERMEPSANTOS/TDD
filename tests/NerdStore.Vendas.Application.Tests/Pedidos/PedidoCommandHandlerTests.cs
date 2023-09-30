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
            mocker.GetMock<IPedidoRepository>().Setup(repository => repository.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await pedidoCommandHandler.Handle(itemCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.Adicionar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);
            // mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
            ;
        }

        [Fact(DisplayName = "Adicionar novo item no pedido existente")]
        [Trait("Categoria", "PedidoCommandHandlerTests")]
        public async Task PedidoCommandHandler_NovoItemAoPedidoExistente_DeveAdicionarComSucesso()
        {
            //Assert
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.GerarNovoPedido(clienteId);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Livro", 2, 100);

            pedido.AdicionarItem(pedidoItem);
            
            var itemCommand = new AdicionarItemCommand(
                   produtoId: Guid.NewGuid(),
                   clienteId: clienteId,
                   nome: "Livro",
                   quantidade: 2,
                   valorUnitario: 100
            );

            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();
            mocker.GetMock<IPedidoRepository>().Setup(repository => repository.ConsultarPedidoPorClientId(clienteId)).Returns(Task.FromResult(pedido));
            mocker.GetMock<IPedidoRepository>().Setup(repository => repository.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await pedidoCommandHandler.Handle(itemCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.Atualizar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);                      
        }

        [Fact(DisplayName = "Atualizar item no pedido existente")]
        [Trait("Categoria", "PedidoCommandHandlerTests")]
        public async Task PedidoCommandHandler_ItemExistente_DeveAtualizarComSucesso()
        {
            //Assert
            var clienteId = Guid.NewGuid();
            var produtoId = Guid.NewGuid();

            var pedido = Pedido.PedidoFactory.GerarNovoPedido(clienteId);
            var pedidoItem = new PedidoItem(produtoId, "Livro", 2, 100);

            pedido.AdicionarItem(pedidoItem);

            var itemCommand = new AdicionarItemCommand(
                   produtoId: produtoId,
                   clienteId: clienteId,
                   nome: "Livro",
                   quantidade: 2,
                   valorUnitario: 100
            );

            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();
            mocker.GetMock<IPedidoRepository>().Setup(repository => repository.ConsultarPedidoPorClientId(clienteId)).Returns(Task.FromResult(pedido));
            mocker.GetMock<IPedidoRepository>().Setup(repository => repository.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await pedidoCommandHandler.Handle(itemCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.Atualizar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IPedidoRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Command inválido")]
        [Trait("Categoria", "PedidoCommandHandlerTests")]
        public async Task PedidoCommandHandler_CommandInvalido_DeveRetornarFalsoELancarEventosDeDominio()
        {
            // Assert
            var itemCommand = new AdicionarItemCommand(
                   produtoId: Guid.Empty,
                   clienteId: Guid.Empty,
                   nome: "",
                   quantidade: 0,
                   valorUnitario: 0
            );
            var mocker = new AutoMocker();
            var pedidoCommandHandler = mocker.CreateInstance<PedidoCommandHandler>();

            //Act
            var result = await pedidoCommandHandler.Handle(itemCommand, CancellationToken.None);

            //Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
