using System.Diagnostics;
using System.Text;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

public record PascalStringToken : PascalToken
{
    /// <summary>
    /// Create a new Pascal string token.
    /// </summary>
    /// <param name="source">The source to get character from.</param>
    public PascalStringToken(Source source)
    {
        char singleQuote = '\'';

        if (source.GetCurrentCharacter() != singleQuote)
        {
            throw new InvalidOperationException($"Expected single quote, found: '{source.GetCurrentCharacter()}'");
        }

        LineNumber = source.LineNumber;
        Position = source.Position;

        StringBuilder textBuffer = new();
        StringBuilder valueBuffer = new();

        char currentCharacter = source.GetNextCharacter();
        _ = textBuffer.Append(singleQuote);
        do
        {
            if (char.IsWhiteSpace(currentCharacter))
            {
                currentCharacter = ' ';
            }

            if ((currentCharacter != singleQuote) && (currentCharacter != Source.EndOfFile))
            {
                _ = textBuffer.Append(currentCharacter);
                _ = valueBuffer.Append(currentCharacter);
                currentCharacter = source.GetNextCharacter();
            }

            if (currentCharacter == singleQuote)
            {
                while ((currentCharacter == singleQuote) && (source.PeekNextCharacter() == singleQuote))
                {
                    _ = textBuffer.Append($"{singleQuote}{singleQuote}");
                    _ = valueBuffer.Append(currentCharacter);
                    _ = source.GetNextCharacter();
                    currentCharacter = source.GetNextCharacter();
                }
            }
        } while ((currentCharacter != singleQuote) && (currentCharacter != Source.EndOfFile));

        if (currentCharacter == singleQuote)
        {
            _ = source.GetNextCharacter();
            _ = textBuffer.Append(singleQuote);
            TokenType = PascalTokenType.String;
            Value = valueBuffer.ToString();
        }
        else
        {
            TokenType = PascalTokenType.Error;
            Value = PascalErrorCode.UnexpectedEof;
        }

        Text = textBuffer.ToString();

        if (TokenType == PascalTokenType.String)
        {
            Debug.Assert(Text.Length >= 2);
            Debug.Assert(Text[0] == singleQuote);
            Debug.Assert(Text[^1] == singleQuote);
        }
        else if (TokenType == PascalTokenType.Error)
        {
            Debug.Assert((Value as PascalErrorCode) == PascalErrorCode.UnexpectedEof);
        }
        else
        {
            Debug.Fail($"Unexpected TokenType: {TokenType.GetType}");
        }
    }
}