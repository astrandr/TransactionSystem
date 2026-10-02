using TransactionSystem.Core;

namespace TransactionSystem.UI.Commands
{
    public class CommandResult
    {
        public CommandResultStatus CommandResultStatus { get; set; }
        public AccountServiceResult AccountServiceResult { get; set; }
        public decimal Amount { get; set; }
        public bool HasReturn { get; set; } = false;
    }
}
