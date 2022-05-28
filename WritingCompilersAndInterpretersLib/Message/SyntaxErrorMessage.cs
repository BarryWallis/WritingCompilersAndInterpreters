namespace WritingCompilersAndInterpretersLib.Message;

public record SyntaxErrorMessage(int LineNumber, int Position, string? TokenText, string ErrorMessage) : Message
{
    public int LineNumber
        = LineNumber > 0 ? LineNumber : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be positive");
    public int Position
        = Position > 0 ? Position : throw new ArgumentOutOfRangeException(nameof(Position), "Must be positive");
    public string ErrorMessage
        = string.IsNullOrWhiteSpace(ErrorMessage)
          ? throw new ArgumentOutOfRangeException(nameof(ErrorMessage), "Must have a value")
          : ErrorMessage;
}
