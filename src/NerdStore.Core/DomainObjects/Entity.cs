
using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }
        private List<Event> _events;
        public IReadOnlyCollection<Event> Notification => _events.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event @event)
        {
            _events ??= new List<Event>();

            _events.Add(@event);
        }

        public void LimparEventos()
        {
            _events?.Clear();
        }

        public void ApagarEvento(Event @event)
        {
            _events?.Remove(@event);
        }
    }
}
