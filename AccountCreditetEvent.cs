using System;

namespace EventStoreTest
{
    internal class AccountCreditetEvent : IAccountEvent
    {
        public Guid AggrateId { get; }
        public CreditTypes CreditType { get; }
        public decimal Sum { get; }

        public AccountCreditetEvent(Guid aggrateId, long Id, CreditTypes creditType, decimal sum)
        {
            this.Id = Id;
            AggrateId = aggrateId;
            CreditType = creditType;
            Sum = sum;
        }


        public void Process(Account account)
        {
            account.Credit(CreditType, Sum);
        }

        public long Id { get; }
    }
}