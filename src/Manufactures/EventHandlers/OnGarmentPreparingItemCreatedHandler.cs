using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentPreparings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentPreparingItemCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentPreparingItem>>
    {
        public Task Handle(OnEntityCreated<GarmentPreparingItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}