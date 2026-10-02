namespace TransactionSystem.UI.Commands.Interpreters
{
    public class DepositCommandInterpreter : ICommandInterpreter
    {
        public InterpreterResult CreateCommand(string commandLine)
        {
            if (commandLine.IndexOf("DEPOSIT :") != 0) return new InterpreterResult() { Matched = false };

            string[] tokens = commandLine.Split(new char[] { ':' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }

            string parametersText = tokens[1];

            string[] parameters = parametersText.Split(new char[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parameters.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }

            var accountNumber = parameters[0];
            var depositAmount = parameters[1];

            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(depositAmount))
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
            }

            if (decimal.TryParse(depositAmount, out decimal depositAmountValue))
            {
                if (depositAmountValue < 0)
                {
                    return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
                }

                return new InterpreterResult()
                {
                    Matched = true,
                    Command = new DepositCommand(accountNumber, depositAmountValue)
                };
            }
            else
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
            }
        }
    }
}
