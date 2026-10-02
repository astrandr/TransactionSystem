namespace TransactionSystem.Core
{
    public interface IAccountRepository
    {
        bool AddAccount(string accountId, Account account);
        bool TryGetAccount(string accountId, out Account account);
    }
}