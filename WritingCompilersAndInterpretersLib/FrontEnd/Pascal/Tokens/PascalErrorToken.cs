namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Tokens;

public record PascalErrorToken : PascalToken
{
    /// <summary>
    /// Create a Pascal error token.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="tokenText">The text of the erroneous token.</param>
    public PascalErrorToken(PascalErrorCode errorCode, string tokenText, Source source)
    {
        LineNumber = source.LineNumber;
        Position = source.Position;
        Text = tokenText;
        TokenType = PascalTokenType.Error;
        Value = errorCode;
    }
}
