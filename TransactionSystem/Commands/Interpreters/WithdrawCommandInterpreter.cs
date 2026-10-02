namespace TransactionSystem.UI.Commands.Interpreters
{
    public class WithdrawCommandInterpreter : ICommandInterpreter
    {
        public InterpreterResult CreateCommand(string commandLine)
        {
            if (commandLine.IndexOf("WITHDRAW :") != 0) return new InterpreterResult() { Matched = false }; 

            string[] tokens = commandLine.Split(new char[] { ':' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }

            string parametersText = tokens[1];

            string[] parameters = parametersText.Split(new char[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parameters.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.IncorrectParameters };
            }

            var accountNumber = parameters[0];
            var withdrawAmount = parameters[1];

            if (decimal.TryParse(withdrawAmount, out decimal withdrawAmountValue))
            {
                if (withdrawAmountValue < 0)
                {
                    return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
                }

                return new InterpreterResult()
                {
                    Matched = true,
                    Command = new WithdrawCommand(accountNumber, withdrawAmountValue)
                };
            }
            else
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
            }
        }
    }
}
