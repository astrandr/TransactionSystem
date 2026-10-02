using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly string accountNumber;
        private readonly decimal depositAmount;

        public WithdrawCommand(string accountNumber, decimal depositAmount)
        {
            this.accountNumber = accountNumber;
            this.depositAmount = depositAmount;
        }

        public CommandResult Execute(IAccountService accountService)
        {
            try
            {
                var serviceResult = accountService.Withdraw(accountNumber, depositAmount);

                if (serviceResult.Status == AccountServiceResultsStatus.Success)
                {
                    return new CommandResult() { CommandResultStatus = CommandResultStatus.Success, Amount = serviceResult.Amount };
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
            catch(Exception ex)
            {
                return new CommandResult() { CommandResultStatus = CommandResultStatus.UknownError };
            }
        }
    }
}
