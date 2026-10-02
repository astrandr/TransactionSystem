using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public interface ICommand
    {
        CommandResult Execute(IAccountService accountService);
    }
}
