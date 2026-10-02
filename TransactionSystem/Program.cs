using TransactionSystem.UI.Commands;

namespace TransactionSystem.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new Context();
            var processor = new ShellProcessor(context);

            Console.WriteLine("Usage:");
            WriteHelpLines();
            Console.WriteLine();

            while(true)
            {
                var commandLine = Console.ReadLine()?.ToUpper();
                if (commandLine != null)
                {

                    if (commandLine.Trim().Equals("QUIT")) return;

                    var executionResultMessage = processor.ProcessCommandLine(commandLine);

                    if (!string.IsNullOrEmpty(executionResultMessage))
                    {
                        Console.WriteLine(executionResultMessage);
                    }
                }
            }
        }

        private static void WriteHelpLines()
        {
            foreach (var helpLine in ShellProcessor.CommandsHelpLines)
            {
                Console.WriteLine(helpLine);
            }
        }
    }
}
