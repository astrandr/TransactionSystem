namespace TransactionSystem.UI.Commands.Interpreters
{
    public interface ICommandInterpreter
    {
        InterpreterResult CreateCommand(string commandLine);
    }
}
