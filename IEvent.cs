using System;

namespace EventStoreTest
{
    internal interface IEvent
    {
        long Id { get; }
        Guid AggrateId { get; }

    }
}