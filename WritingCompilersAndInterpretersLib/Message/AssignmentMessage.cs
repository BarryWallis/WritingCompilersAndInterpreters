namespace WritingCompilersAndInterpretersLib.Message;

public record AssignmentMessage(int LineNumber, string VariableName, object Value) : Message
{
    public int LineNumber { get; }
        = LineNumber > 0 ? LineNumber
                         : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be positive");
    public string VariableName { get; }
        = string.IsNullOrWhiteSpace(VariableName)
          ? throw new ArgumentException("Must have a value", nameof(VariableName))
          : VariableName;
    public object Value { get; } = Value is not null ? Value : throw new ArgumentNullException(nameof(Value));
}
