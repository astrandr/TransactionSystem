namespace TransactionSystem.Core
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository accountRepository;
        private static readonly Dictionary<string, object> accountLocks = new Dictionary<string, object>();

        public AccountService()
        {
            accountRepository = new AccountRepository();
        }

        public AccountServiceResult CreateAccount(string userName, string accountNumber, decimal depositAmount)
        {
            lock (accountLocks)
            {
                if (accountRepository.TryGetAccount(accountNumber, out Account account))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.ExistingAccount };
                }

                if (depositAmount <= 0)
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
                }

                accountRepository.AddAccount(accountNumber, new Account() { Number = accountNumber, User = userName, Amount = depositAmount });
                accountLocks.Add(accountNumber, new object());
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = 0 };
        }

        public AccountServiceResult Deposit(string accountNumber, decimal depositAmount)
        {
            if (depositAmount <= 0)
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
            }

            Account account;

            lock (accountLocks)
            {
                if (!accountRepository.TryGetAccount(accountNumber, out account))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
                }
            }

            lock (accountLocks[accountNumber])
            {
                account.Amount += depositAmount;
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = depositAmount };
        }

        public AccountServiceResult Withdraw(string accountNumber, decimal withdrawAmount)
        {

            Account account;

            lock (accountLocks)
            {
                if (!accountRepository.TryGetAccount(accountNumber, out account))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
                }
            }
            
            lock (accountLocks[accountNumber])
            {
                if (withdrawAmount > account.Amount)
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.InsufficientFunds };
                }

                account.Amount -= withdrawAmount;

                return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success };
            }
        }

        public AccountServiceResult GetAccountBalance(string accountNumber)
        {

            Account account;

            lock (accountLocks)
            {
                if (!accountRepository.TryGetAccount(accountNumber, out account))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
                }
            }

            decimal accountbalance = 0;
            lock (accountLocks[accountNumber])
            {
                accountbalance = account.Amount;
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = accountbalance };
        }
    }
}
