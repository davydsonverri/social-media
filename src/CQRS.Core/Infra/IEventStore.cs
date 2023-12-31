﻿using CQRS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Infra
{
    public interface IEventStore
    {
        Task SaveEventAsync(Did aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Did aggregateId);
        Task<List<Did>> GetAggregateIdsAsync();
    }
}
