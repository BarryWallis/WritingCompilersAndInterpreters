using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// Create tokens based on the next character in the source.
/// </summary>
public class TokenFactory
{
    private readonly Source _source;

    /// <summary>
    /// Create a new <see cref="TokenFactory"/> by reading characters from <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The <see cref="Source"/> to read characters from.</param>
    public TokenFactory(Source source) => _source = source;

    /// <summary>
    /// Create a new token based on the current <see cref="Source"/> character.
    /// </summary>
    /// <returns>The new <see cref="Token"/>.</returns>
    public virtual Token Create()
    {
        SkipWhiteSpace();

        Token token;
        char currentCharacter = _source.GetCurrentCharacter();
        if (currentCharacter == Source.EndOfFile)
        {
            token = new EndOfFileToken(_source);
        }
        else if (char.IsLetter(currentCharacter))
        {
            token = new PascalWordToken(_source);
        }
        else if (char.IsDigit(currentCharacter))
        {
            token = new PascalNumberToken(_source);
        }
        else if (currentCharacter == '\'')
        {
            token = new PascalStringToken(_source);
        }
        else if (PascalTokenType.LookupSpecialSymbols.ContainsKey(currentCharacter.ToString()))
        {
            token = new PascalSpecialSymbolToken(_source);
        }
        else
        {
            token = new PascalErrorToken(PascalErrorCode.InvalidCharacter, currentCharacter.ToString(), _source);
            _ = _source.GetNextCharacter();
        }

        return token;
    }

    private void SkipWhiteSpace()
    {
        char currentCharacter = _source.GetCurrentCharacter();
        while (char.IsWhiteSpace(currentCharacter) || (currentCharacter == '{'))
        {
            if (currentCharacter == '{')
            {
                do
                {
                    currentCharacter = _source.GetNextCharacter();
                } while (currentCharacter is not '}' and not Source.EndOfFile);

                if (currentCharacter == '}')
                {
                    currentCharacter = _source.GetNextCharacter();
                }
            }
            else
            {
                currentCharacter = _source.GetNextCharacter();
            }
        }
    }
}
