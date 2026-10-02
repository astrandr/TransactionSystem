using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class Context
    {
        public IAccountService AccountService = new AccountService();
    }
}
