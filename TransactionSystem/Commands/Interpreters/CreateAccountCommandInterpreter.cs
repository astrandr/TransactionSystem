namespace TransactionSystem.UI.Commands.Interpreters
{
    public class CreateAccountCommandInterpreter : ICommandInterpreter
    {
        public InterpreterResult CreateCommand(string commandLine)
        {
            if (commandLine.IndexOf("CREATE :") != 0) return new InterpreterResult() { Matched = false }; 

            string[] tokens = commandLine.Split(new char[] { ':' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }

            string parametersText = tokens[1];

            string[] parameters = parametersText.Split(new char[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parameters.Length != 3)
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.IncorrectParameters };
            }

            var userName = parameters[0];
            var accountNumber = parameters[1];
            var depositAmount = parameters[2];

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(accountNumber))
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.IncorrectParameters };
            }

            if (decimal.TryParse(depositAmount, out decimal depositAmountValue))
            {
                if (depositAmountValue <= 0)
                {
                    return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
                }

                return new InterpreterResult()
                {
                    Matched = true,
                    Command = new CreateAccountCommand(userName, accountNumber, depositAmountValue)
                };
            }
            else
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }
        }
    }
}
