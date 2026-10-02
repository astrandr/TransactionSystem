using TransactionSystem.Core;
using TransactionSystem.UI.Commands;
using TransactionSystem.UI.Commands.Interpreters;

namespace TransactionSystem.UI
{
    public class ShellProcessor
    {
        private readonly List<ICommandInterpreter> interpreters;
        private readonly Context context;

        private const string UnknownCommandMessage = "Unknown command";
        private const string InvalidCommandMessage = "Invalid command structure/parameters";

        public static readonly IEnumerable<string> CommandsHelpLines = new List<string>()
        {
            "create : {user name}, {account number}, {initial deposit amount}",
            "balance : {account number} ",
            "deposit : {account number}, {deposit amount}",
            "withdraw : {account number}, {withdrawal amount}",
            "quit - will exit"
        };

        public ShellProcessor(Context context)
        {
            interpreters = new List<ICommandInterpreter>();

            interpreters.Add(new CreateAccountCommandInterpreter());
            interpreters.Add(new GetBalanceCommandInterpreter());
            interpreters.Add(new DepositCommandInterpreter());
            interpreters.Add(new WithdrawCommandInterpreter());

            this.context = context;
        }

        public string ProcessCommandLine(string commandLine)
        {
            var foundMatch = FindCommand(commandLine);

            if (foundMatch == null) return UnknownCommandMessage;

            if (foundMatch.Command != null)
            {
                var commandResult = foundMatch.Command.Execute(context);

                if (commandResult.CommandResultStatus != CommandResultStatus.Success)
                {
                    return TranslateCommandResult(commandResult);
                }
                else if(commandResult.HasReturn)
                {
                    return $":{commandResult.Amount}";
                }

                return string.Empty;
            }
            else
            {
                return InvalidCommandMessage;
            }
        }

        private InterpreterResult FindCommand(string commandLine)
        {
            foreach (var interpreter in interpreters)
            {
                var result = interpreter.CreateCommand(commandLine);
                if (result.Matched) return result;
            }

            return null;
        }

        private string TranslateCommandResult(CommandResult result)
        {
            switch (result.CommandResultStatus)
            {
                case CommandResultStatus.ServiceError: return TranslateServiceResult(result.AccountServiceResult);
                case CommandResultStatus.InvalidParameter: return "Invalid parameter.";
                case CommandResultStatus.UknownError: return "Unknow error has occured.";
            }
            return string.Empty;
        }

        private string TranslateServiceResult(AccountServiceResult result)
        {
            switch (result.Status)
            {
                case AccountServiceResultsStatus.InsufficientFunds: return "Insufficient funds";
                case AccountServiceResultsStatus.NonExistingAccount: return "Non existing account";
                case AccountServiceResultsStatus.ExistingAccount: return "Existing account";
                case AccountServiceResultsStatus.InvalidAmountValue: return "Invalid amount";
            }

            return string.Empty;
        }
    }
}
