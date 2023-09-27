using NerdStore.Vendas.Application.Pedidos.Commands;
using NerdStore.Vendas.Application.Pedidos.Validations;
using NerdStore.Vendas.Domain;
using NuGet.Frameworks;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemCommandTests
    {
        [Fact(DisplayName = "Item command deve ser valido")]
        [Trait("Categoria", "AdicionarItemCommand")]
        public void AdicionarItemCommand_CommandDeveSerValido_DevePassarPelaValidacao()
        {
            //Arrange
            var itemCommand = new AdicionarItemCommand(
                   produtoId: Guid.NewGuid(),
                   clienteId: Guid.NewGuid(),
                   nome: "Livro",
                   quantidade: 2,
                   valorUnitario: 100
            );

            //Act
            var ehValido = itemCommand.ValidarSeEhValido();

            //Assert
            Assert.True(ehValido);
        }

        [Fact(DisplayName = "Item command deve ser invalido")]
        [Trait("Categoria", "AdicionarItemCommand")]
        public void AdicionarItemCommand_CommandDeveSerInvalido_NaoDevePassarPelaValidacao()
        {
            //Arrange
            var itemCommand = new AdicionarItemCommand(
                   produtoId: Guid.Empty,
                   clienteId: Guid.Empty,
                   nome: "",
                   quantidade: 0,
                   valorUnitario: 0
            );

            //Act
            var ehValido = itemCommand.ValidarSeEhValido();
            //Assert

            var erros = itemCommand.ValidationResult.Errors.Select(erro => erro.ErrorMessage);

            Assert.False(ehValido);
            Assert.Contains(AdicionarItemCommandValidation.IdClienteErroMsg, erros);
            Assert.Contains(AdicionarItemCommandValidation.NomeErroMsg, erros);
            Assert.Contains(AdicionarItemCommandValidation.ValorErroMsg, erros);
            Assert.Contains(AdicionarItemCommandValidation.IdProdutoErroMsg, erros);
            Assert.Contains(AdicionarItemCommandValidation.QtdMinErroMsg, erros);
        }

    }
}
