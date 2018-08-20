using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStoreTest
{
    internal class AccountRepo
    {
        private List<IAccountEvent> _events = new List<IAccountEvent>();

        public void Save(Account account)
        {
            _events.AddRange(account.UnCommitedEvents);
            account.UnCommitedEvents.Clear();
        }

        public Account GetById(Guid newAccountId)
        {
            var events = _events.Where(e => e.AggrateId == newAccountId).OrderBy(e => e.Id).ToList();
            return new Account(newAccountId,events);
        }
    }
}