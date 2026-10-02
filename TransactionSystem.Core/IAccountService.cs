namespace TransactionSystem.Core
{
    public interface IAccountService
    {
        AccountServiceResult CreateAccount(string userName, string accountNumber, decimal depositAmount);

        AccountServiceResult GetAccountBalance(string accountNumber);

        AccountServiceResult Deposit(string accountNumber, decimal depositAmount);

        AccountServiceResult Withdraw(string accountNumber, decimal depositAmount);

        AccountServiceResult Transfer(string srcAccountNumber, string destAccountNumber, decimal depositAmount);
    }
}
