using FluentValidation.Results;
using MediatR;

namespace NerdStore.Core.Messages
{
    public abstract class Command : IRequest<bool>
    {
        public ValidationResult ValidationResult { get; set; }
        public DateTime Timestamp { get; set; }

        protected Command()
        {
               Timestamp = DateTime.UtcNow;
        }

        public abstract bool ValidarSeEhValido();
    }
}
