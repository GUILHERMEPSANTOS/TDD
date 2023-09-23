using FluentValidation.Results;
using NerdStore.Vendas.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public string Codigo { get; private set; }
        public bool Ativo { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDesconto TipoDesconto { get; private set; }
        public DateTime DataValidade { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        private ValidationResult ValidationResult { get; set; }



        public Voucher(string codigo, bool ativo, int quantidade, TipoDesconto tipoDesconto, decimal? valorDesconto, decimal? percentualDesconto, DateTime dataValidade)
        {
            Codigo = codigo;
            Ativo = ativo;
            Quantidade = quantidade;
            TipoDesconto = tipoDesconto;
            ValorDesconto = valorDesconto;
            PercentualDesconto = percentualDesconto;
            DataValidade = dataValidade;
        }


        public ValidationResult ValidarSeEhValido()
        {
            ValidationResult = new VoucherValidation().Validate(this);
            
            return ValidationResult;
        }
    }
}

