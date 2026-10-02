using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class TransferCommand : ICommand
    {
        private readonly string sourceAccountNumber;
        private readonly string destAccountNumber;
        private readonly decimal transferAmount;

        public TransferCommand(string sourceAccountNumber, string destAccountNumber, decimal transferAmount)
        {
            this.sourceAccountNumber = sourceAccountNumber;
            this.destAccountNumber = destAccountNumber;
            this.transferAmount = transferAmount;
        }

        public CommandResult Execute(IAccountService accountService)
        {
            var serviceResult = accountService.Transfer(sourceAccountNumber, destAccountNumber, transferAmount);

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
    }
}
