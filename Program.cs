using System;
using System.Collections;
using System.Net;
using System.Text;
using EventStore.ClientAPI;

namespace EventStoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var newAccount = new Account("Timex");
            newAccount.Credit(CreditTypes.B2C, 100);
            newAccount.Credit(CreditTypes.B2C, 50);
            newAccount.Credit(CreditTypes.B2C, 200);

            newAccount.Debit(DebitType.SimCards, 50);

            var repo = new AccountRepo();
            repo.Save(newAccount);

            var timexAccount = repo.GetById(newAccount.Id);
            timexAccount.Debit(DebitType.SimCards, 100);
            timexAccount.Debit(DebitType.SimCards, 100);


            repo.Save(timexAccount);
            var n = repo.GetById(newAccount.Id);

            Recipt recipt = null;
            try
            {
                recipt = n.Withdraw(101);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            recipt = n.Withdraw(50);

        }
    }
}