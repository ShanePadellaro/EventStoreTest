using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStoreTest
{
    internal class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; }
        public decimal Balance { get; private set; }
        private long _version;
        public IList<IAccountEvent> UnCommitedEvents = new List<IAccountEvent>();

        public Account(Guid id, List<IAccountEvent> events)
        {
            Id = id;
            events.ForEach(e => e.Process(this));
            _version = events.Max(e => e.Id);
            UnCommitedEvents.Clear();
        }

        public Account(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public void Credit(CreditTypes creditType, decimal sum)
        {
            Balance += sum;
            _version++;
            UnCommitedEvents.Add(new AccountCreditetEvent(Id, _version, creditType, sum));
        }

        public void Debit(DebitType debitType, decimal sum)
        {
            Balance -= sum;
            _version++;
            UnCommitedEvents.Add(new AccountDebitedEvent(Id, _version, debitType, sum));
        }

        public Recipt Withdraw(decimal sum)
        {
            if (sum > Balance)
                throw new Exception("Account balance to low");

            Balance -= sum;
            _version++;
            UnCommitedEvents.Add(new AccountWithdrawalEvent(Id, _version, sum));
            
            return new Recipt(sum);
        }
    }

    internal class AccountWithdrawalEvent : IAccountEvent
    {
        public AccountWithdrawalEvent(Guid id, long version, decimal sum)
        {
            AggrateId = id;
            Id = version;
            Sum = sum;
        }

        public long Id { get; }
        public decimal Sum { get; }
        public Guid AggrateId { get; }

        public void Process(Account account)
        {
            account.Withdraw(Sum);
        }
    }

    internal class AccountDebitedEvent : IAccountEvent
    {
        public AccountDebitedEvent(Guid id, long version, DebitType debitType, decimal sum)
        {
            Id = version;
            DebitType = debitType;
            Sum = sum;
            AggrateId = id;
        }

        public long Id { get; }
        public DebitType DebitType { get; }
        public decimal Sum { get; }
        public Guid AggrateId { get; }

        public void Process(Account account)
        {
            account.Debit(DebitType, Sum);
        }
    }
}