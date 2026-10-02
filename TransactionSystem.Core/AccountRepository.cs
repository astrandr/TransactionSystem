using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace TransactionSystem.Core
{
    public class AccountRepository
    {
        private Dictionary<string, Account> accounts = new Dictionary<string, Account>();

        public bool TryGetAccount(string accountId, out Account account)
        {
            if (accounts.ContainsKey(accountId))
            {
                account = accounts[accountId];
                return true;
            }

            account = null;
            return false;
        }

        public bool AddAccount(string accountId, Account account)
        {
            if (!accounts.ContainsKey(accountId))
            {
                accounts.Add(accountId, account);
                return true;
            }

            return false;
        }
    }
}
