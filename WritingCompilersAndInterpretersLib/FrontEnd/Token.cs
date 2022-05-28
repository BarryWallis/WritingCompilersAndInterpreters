using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// The framework class that represents a token returned by the scanner.
/// </summary>
public abstract record Token(int LineNumber, int Position, PascalTokenType? TokenType, string? Text, object? Value)
{
    protected Token() : this(0, 0, null, null, null)
    {
    }

    public int LineNumber { get; init; }
        = LineNumber >= 0 ? LineNumber : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be non-negative");
    public int Position { get; init; }
        = Position >= 0 ? Position : throw new ArgumentOutOfRangeException(nameof(Position), "Must be non-negative");
    public PascalTokenType? TokenType { get; protected set; } = TokenType;
    public object? Value { get; protected set; } = Value;
}