namespace WritingCompilersAndInterpretersLib.FrontEnd;

/// <summary>
/// The framework class that represents a token returned by the scanner.
/// </summary>
public /*abstract*/ record Token(int LineNumber, int Position, string? Text, object? Value)
{
    public int LineNumber { get; } 
        = LineNumber >= 0 ? LineNumber : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be non-negative");
    public int Position { get; }
        = Position >= 0 ? Position : throw new ArgumentOutOfRangeException(nameof(Position), "Must be non-negative");

    /// <summary>
    /// Create a new empty token.
    /// </summary>
    protected Token() : this(0, 0, null, null)
    {
    }
}