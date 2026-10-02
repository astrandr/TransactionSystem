using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class GetBalanceCommand : ICommand
    {

        private readonly string accountNumber;

        public GetBalanceCommand(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }

        public CommandResult Execute(IAccountService accountService)
        {
            var serviceResult = accountService.GetAccountBalance(accountNumber);

            if (serviceResult.Status == AccountServiceResultsStatus.Success)
            {
                return new CommandResult() { CommandResultStatus = CommandResultStatus.Success, Amount = serviceResult.Amount, HasReturn = true };
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
