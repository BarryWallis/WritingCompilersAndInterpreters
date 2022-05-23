using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

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
        char currentCharacter = GetCurrentCharacter();
        Token token = currentCharacter switch
        {
            Source.EndOfFile => new EndOfFileToken(),
            _ => ExtractToken(_source)// Ignore all tokens until EOF
        };
        return token;

    }

    private Token ExtractToken(Source source)
    {
        string text = $"{GetCurrentCharacter()}";
        object? value = null;
        _ = source.GetNextCharacter();
        return new Token(_source.LineNumber, _source.Position, text, value);
    }

    private char GetCurrentCharacter() => _source.GetCurrentCharacter();
}
