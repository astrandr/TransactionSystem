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
            processor.WriteHelpLines();
            Console.WriteLine();

            while(true)
            {
                var commandLine = Console.ReadLine().ToUpper();
                
                if (commandLine.IndexOf("QUIT") >= 0) return;

                var executionResultMessage = processor.ProcessCommandLine(commandLine);
               
                if (!string.IsNullOrEmpty(executionResultMessage))
                {
                    Console.WriteLine(executionResultMessage);
                }
            }
        }
    }
}
