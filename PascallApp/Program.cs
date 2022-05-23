using System.Text;
using System.Xml.Linq;

namespace PascalApp;

internal class Program
{
    /// <summary>
    /// The main method.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments: "compile" or "execute" followed by optional flags "i" for printing the intermediate code and 
    /// "x" for printing a cross-reference.
    /// </param>
    private static void Main(string[] args)
    {
        try
        {
            string operation = args[0];
            if (!(operation.Equals("compile", StringComparison.OrdinalIgnoreCase)
                  || operation.Equals("execute", StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception();
            }

            int i = 0;
            StringBuilder flags = new();
            while ((++i < args.Length) && (args[i][0] == '-'))
            {
                _ = flags.Append(args[i].AsSpan(1));
            }

            if (i < args.Length)
            {
                string filePath = args[i];
                _ = new Pascal(operation, filePath, flags.ToString());
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception)
        {
            const string flagsMessage = "[-ix]";
            const string usageMessage = $"Usage: PascalApp execute|compile {flagsMessage} <source file path>";

            Console.Error.WriteLine(usageMessage);
        }
    }
}
