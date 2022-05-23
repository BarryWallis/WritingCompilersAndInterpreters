using WritingCompilersAndInterpretersLib.Message;

namespace PascalApp;
public class SourceMessageListener : IObserver<Message>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    /// <inheritdoc/>
    public void OnNext(Message value)
    {
        switch (value)
        {
            case SourceLineMessage sourceLineMessage:
                Console.WriteLine($"{sourceLineMessage.LineNumber,3:D3} {sourceLineMessage.Line}");
                break;
        }
    }
}
