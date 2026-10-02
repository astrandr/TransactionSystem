using System.Collections.Concurrent;

namespace TransactionSystem.Core
{
    public class AccountService : IAccountService
    {
        private readonly ConcurrentDictionary<string, object> accountLocks = new();
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
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

                accountLocks.TryAdd(accountNumber, new object());
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = depositAmount };
        }

        public AccountServiceResult Deposit(string accountNumber, decimal depositAmount)
        {
            if (depositAmount <= 0)
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
            }

            Account account;

            if (!accountRepository.TryGetAccount(accountNumber, out account))
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
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

            if (withdrawAmount <= 0)
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
            }


            if (!accountRepository.TryGetAccount(accountNumber, out account))
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
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


            if (!accountRepository.TryGetAccount(accountNumber, out account))
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingAccount };
            }

            decimal accountbalance = 0;
            lock (accountLocks[accountNumber])
            {
                accountbalance = account.Amount;
            }

            return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success, Amount = accountbalance };
        }

        public AccountServiceResult Transfer(string srcAccountNumber, string destAccountNumber, decimal transferAmount)
        {
            if (transferAmount <= 0)
            {
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.InvalidAmountValue };
            }

            var firstLockAccountNumber = string.CompareOrdinal(srcAccountNumber, destAccountNumber) < 0 ? srcAccountNumber : destAccountNumber;

            var secondLockAccountNumber = string.CompareOrdinal(srcAccountNumber, destAccountNumber) < 0 ? destAccountNumber : srcAccountNumber;

            object firstLock, secondLock;

            if (!accountLocks.ContainsKey(srcAccountNumber))
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingSourceAccount };

            if (!accountLocks.ContainsKey(destAccountNumber))
                return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingDestAccount };

            firstLock = accountLocks[firstLockAccountNumber];
            secondLock = accountLocks[secondLockAccountNumber];

            lock (firstLock)
            {
                lock (secondLock)
                {
                    if (!accountRepository.TryGetAccount(srcAccountNumber, out Account srcAccount))
                    {
                        return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingSourceAccount };
                    }

                    if (!accountRepository.TryGetAccount(destAccountNumber, out Account destAccount))
                    {
                        return new AccountServiceResult() { Status = AccountServiceResultsStatus.NonExistingDestAccount };
                    }


                    if (transferAmount > srcAccount.Amount)
                    {
                        return new AccountServiceResult() { Status = AccountServiceResultsStatus.InsufficientFunds };
                    }

                    destAccount.Amount += transferAmount;
                    srcAccount.Amount -= transferAmount;

                    return new AccountServiceResult() { Status = AccountServiceResultsStatus.Success };
                }
            }
        }
    }
}
