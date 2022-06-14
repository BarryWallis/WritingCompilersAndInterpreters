namespace WritingCompilersAndInterpretersLib.Message
{
    public record RuntimeErrorMessage(string ErrorMessage, int? LineNumber) : Message
    {
        public string ErrorMessage = string.IsNullOrWhiteSpace(ErrorMessage)
                                     ? throw new ArgumentException("Must have a value", nameof(ErrorMessage))
                                     : ErrorMessage;
        public int? LineNumber = LineNumber > 0
                                ? LineNumber
                                : throw new ArgumentOutOfRangeException(nameof(LineNumber));
    }
}