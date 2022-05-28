using System.Diagnostics;
using System.Text;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

public record PascalWordToken : PascalToken
{
    /// <summary>
    /// Create a new Pascal word token.
    /// </summary>
    /// <param name="source">The source to get characters from.</param>
    public PascalWordToken(Source source)
    {
        if (!char.IsLetter(source.GetCurrentCharacter()))
        {
            throw new InvalidOperationException($"Expected letter, found: '{source.GetCurrentCharacter()}'");
        }

        LineNumber = source.LineNumber;
        Position = source.Position;

        StringBuilder textBuffer = new();
        char currentCharacter = source.GetCurrentCharacter();
        while (char.IsLetterOrDigit(currentCharacter))
        {
            _ = textBuffer.Append(currentCharacter);
            currentCharacter = source.GetNextCharacter();
        }

        Text = textBuffer.ToString();
        string normalizedText = Text.ToLowerInvariant();
        TokenType = PascalTokenType.ReservedWords.Contains(normalizedText)
                    ? PascalTokenType.TokenTypes.First(t => t.Text == normalizedText)
                    : PascalTokenType.Identifier;
        Value = null;

        Debug.Assert(Text.Length > 0);
        Debug.Assert(Text.All(c => char.IsLetterOrDigit(c)));
    }
}