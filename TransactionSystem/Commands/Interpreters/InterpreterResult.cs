namespace TransactionSystem.UI.Commands.Interpreters
{
    public class InterpreterResult
    {
        public bool Matched { get; set; }
        public ICommand? Command { get; set; }

        public InterpreterResultStatus Status { get; set; }
    }
}
