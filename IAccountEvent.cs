namespace EventStoreTest
{
    internal interface IAccountEvent : IEvent
    {
        void Process(Account account);
    }
}