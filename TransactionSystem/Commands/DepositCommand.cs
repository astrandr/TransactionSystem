using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class DepositCommand : ICommand
    {
        private readonly string accountNumber;
        private readonly decimal depositAmount;

        public DepositCommand(string accountNumber, decimal depositAmount)
        {
            this.accountNumber = accountNumber;
            this.depositAmount = depositAmount;
        }

        public CommandResult Execute(Context context)
        {
            var serviceResult = context.AccountService.Deposit(accountNumber, depositAmount);

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
