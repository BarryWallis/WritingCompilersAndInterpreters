using WritingCompilersAndInterpretersLib.Message;

namespace PascalApp;
public class ParserMessageListener : IObserver<Message>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    /// <inheritdoc/>
    public void OnNext(Message value)
    {
        switch (value)
        {
            case ParserSummaryMessage parserSummaryMessage:
                Console.WriteLine();
                Console.WriteLine($"{parserSummaryMessage.NumberOfLines,20} source lines.");
                Console.WriteLine($"{parserSummaryMessage.ErrorCount,20} syntax errors.");
                Console.WriteLine($"{parserSummaryMessage.ElapsedTIme,20:F2} seconds total parsing time. ");
                break;
        }
    }
}
