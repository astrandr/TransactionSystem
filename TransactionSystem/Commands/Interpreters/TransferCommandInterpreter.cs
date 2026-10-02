namespace TransactionSystem.UI.Commands.Interpreters
{
    public class TransferCommandInterpreter : ICommandInterpreter
    {
        public InterpreterResult CreateCommand(string commandLine)
        {
            if (commandLine.IndexOf("TRANSFER :") != 0) return new InterpreterResult() { Matched = false }; 

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

            var sourceAccountNumber = parameters[0];
            var destinationAccountNumber = parameters[1];
            var transferAmount = parameters[2];

            if (decimal.TryParse(transferAmount, out decimal transferAmountValue))
            {
                if (transferAmountValue < 0)
                {
                    return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
                }

                return new InterpreterResult()
                {
                    Matched = true,
                    Command = new TransferCommand(sourceAccountNumber, destinationAccountNumber, transferAmountValue)
                };
            }
            else
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.InvalidValue };
            }
        }
    }
}
