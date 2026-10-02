using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class CreateAccountCommand : ICommand
    {
        private readonly string userName;
        private readonly string accountNumber;
        private readonly decimal depositAmount;

        public CreateAccountCommand(string userName, string accountNumber, decimal depositAmount)
        {
            this.userName = userName;
            this.accountNumber = accountNumber;
            this.depositAmount = depositAmount;
        }

        public CommandResult Execute(IAccountService accountService)
        {
            var serviceResult = accountService.CreateAccount(userName, accountNumber, depositAmount);
            if (serviceResult.Status == AccountServiceResultsStatus.Success)
            {
                return new CommandResult() { CommandResultStatus = CommandResultStatus.Success };
            }
            else
            {
                return new CommandResult()
                {
                    CommandResultStatus = CommandResultStatus.ServiceError,
                    AccountServiceResult = serviceResult
                };
            }
        }
    }
}
