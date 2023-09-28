using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Vendas.Data.Extensions;

namespace NerdStore.Vendas.Data
{
    public class VendaContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        public VendaContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            if (success) await _mediator.PublishEvents<DbContext>(this);

            return success;
        }
    }
}
