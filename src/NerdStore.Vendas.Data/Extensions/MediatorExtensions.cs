using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data.Extensions
{
    public static class MediatorExtensions
    {
        public static Task PublishEvents<TDbContext>(this IMediator mediator, TDbContext context) where TDbContext : DbContext
        {
            var domainEntities = context.ChangeTracker
                 .Entries<Entity>()
                 .Where(entityEntry => entityEntry.Entity.Notification is not null && entityEntry.Entity.Notification.Any())
                 .Select(entityEntry => entityEntry.Entity);

            if (!domainEntities.Any()) return Task.CompletedTask;

            var domainEvents = domainEntities
                    .Select(domainEntity => domainEntity.Notification)
                    .ToList();

            if(!domainEvents.Any()) return Task.CompletedTask;

            domainEntities?.ToList()
                    .ForEach(entity => entity.LimparEventos());

            var task = domainEvents
                  .Select(async (domainEvents) =>
                  {
                      await mediator.Publish(domainEvents);
                  });

            return Task.WhenAll(task);
        }
    }
}
