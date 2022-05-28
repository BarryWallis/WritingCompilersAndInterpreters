using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;

namespace WritingCompilersAndInterpretersLib.Message;

public record TokenMessage(int LineNumber, int Position, PascalTokenType TokenType, string Text, object? Value) : Message
{
    public int LineNumber = LineNumber > 0 ? LineNumber
                                           : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be positive");
    public int Position = Position >= 0 ? Position
                                       : throw new ArgumentOutOfRangeException(nameof(Position), "Must be non-negative");
}