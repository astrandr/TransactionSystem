namespace TransactionSystem.UI.Commands.Interpreters
{
    public class GetBalanceCommandInterpreter : ICommandInterpreter
    {
        public InterpreterResult CreateCommand(string commandLine)
        {
            if (commandLine.IndexOf("BALANCE :") != 0) return new InterpreterResult() { Matched = false };

            string[] tokens = commandLine.Split(new char[] { ':' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                return new InterpreterResult() { Matched = true, Command = null };
            }

            string parametersText = tokens[1];

            string[] parameters = parametersText.Split(new char[] { ',' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parameters.Length != 1)
            {
                return new InterpreterResult() { Matched = true, Command = null, Status = InterpreterResultStatus.IncorrectParameters };
            }

            var accountNumber = parameters[0];
  
            return new InterpreterResult()
            {
                Matched = true,
                Command = new GetBalanceCommand(accountNumber)
            };
        }
    }
}
