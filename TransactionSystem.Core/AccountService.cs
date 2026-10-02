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
                if (accountRepository.Accounts.ContainsKey(accountNumber))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.ExistingAccount };
                }

                if(depositAmount <= 0)
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
                }

                accountRepository.Accounts.Add(accountNumber, new Account() { Number = accountNumber, User = userName, Amount = depositAmount });
                accountLocks.Add(accountNumber, new object());
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = 0 };
        }

        public AccountServiceResult Deposit(string accountNumber, decimal depositAmount)
        {
            lock (accountLocks)
            {
                if (!accountRepository.Accounts.ContainsKey(accountNumber))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
                }
            }

            lock (accountLocks[accountNumber])
            {
                accountRepository.Accounts[accountNumber].Amount += depositAmount;
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = depositAmount };
        }

        public AccountServiceResult Withdraw(string accountNumber, decimal withdrawAmount)
        {
            lock (accountLocks)
            {
                if (!accountRepository.Accounts.ContainsKey(accountNumber))
                {
                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
                }
            }

            lock (accountLocks[accountNumber])
            {
                var actualWithdrawAmmount = withdrawAmount < accountRepository.Accounts[accountNumber].Amount ? withdrawAmount : accountRepository.Accounts[accountNumber].Amount;

                accountRepository.Accounts[accountNumber].Amount -= actualWithdrawAmmount;

                return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = actualWithdrawAmmount };
            }
        }

        public AccountServiceResult GetAccountBalance(string accountNumber)
        {
            if (!accountRepository.Accounts.ContainsKey(accountNumber))
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = accountRepository.Accounts[accountNumber].Amount };
        }
    }
}
