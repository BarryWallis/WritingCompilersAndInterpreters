using System.Diagnostics;
using System.Text;

using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.Message;

namespace PascalApp;
public class ParserMessageListener : IObserver<Message>
{
    public void OnCompleted() => throw new NotImplementedException();

    public void OnError(Exception error) => throw new NotImplementedException();

    /// <inheritdoc/>
    public void OnNext(Message message)
    {
        switch (message)
        {
            case ParserSummaryMessage parserSummaryMessage:
                Console.WriteLine();
                Console.WriteLine($"{parserSummaryMessage.NumberOfLines,20} source lines.");
                Console.WriteLine($"{parserSummaryMessage.ErrorCount,20} syntax errors.");
                Console.WriteLine($"{parserSummaryMessage.ElapsedTIme,20:F2} seconds total parsing time. ");
                break;
            case TokenMessage tokenMessage:
                Console.WriteLine($">>> {tokenMessage.TokenType.Text,-15} line={tokenMessage.LineNumber,3:D3}, " +
                    $"pos={tokenMessage.Position,2}, text=\"{tokenMessage.Text}\"");
                if (tokenMessage.Value is not null)
                {
                    Debug.Assert(tokenMessage.Value.ToString() is not null);
                    string tokenValue = tokenMessage.Value.ToString()!;
                    if (tokenMessage.TokenType == PascalTokenType.String)
                    {
                        tokenValue = tokenMessage.TokenType == PascalTokenType.String ? $"\"{tokenValue!}\"" 
                                                                                             : tokenValue!;
                    }
                    Console.WriteLine($">>>                 value={tokenValue}");
                }
                break;
            case SyntaxErrorMessage syntaxErrorMessage:
                const int prefixWidth = 5;
                int spaceCount = prefixWidth + syntaxErrorMessage.Position;
                StringBuilder flagBuffer = new();
                _ = flagBuffer.Append(new string(' ', spaceCount - 1)).Append($"^\n*** {syntaxErrorMessage.ErrorMessage}");
                if (syntaxErrorMessage.TokenText is not null)
                {
                    _ = flagBuffer.Append($" [at \"{syntaxErrorMessage.TokenText}\"]");
                }
                Console.WriteLine(flagBuffer.ToString());
                break;
        }
    }
}
