using FluentValidation.Results;

namespace NerdStore.Core.Messages
{
    public abstract class Command
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
