using NerdStore.Core.DomainObjects;
using System.Threading.Tasks;

namespace NerdStore.Core.Data
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
