using System.Collections.Concurrent;

namespace TransactionSystem.Core
{
    public class AccountRepository : IAccountRepository
    {
        private ConcurrentDictionary<string, Account> accounts = new ConcurrentDictionary<string, Account>();

        public bool TryGetAccount(string accountId, out Account account)
        {
            return accounts.TryGetValue(accountId, out account);
        }

        public bool AddAccount(string accountId, Account account)
        {
            return accounts.TryAdd(accountId, account);
        }
    }
}
