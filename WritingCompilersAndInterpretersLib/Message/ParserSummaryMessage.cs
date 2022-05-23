namespace WritingCompilersAndInterpretersLib.Message;

public record ParserSummaryMessage(int NumberOfLines, int ErrorCount, float ElapsedTIme) : Message
{
    public int NumberOfLines { get; }
        = NumberOfLines >= 0
          ? NumberOfLines
          : throw new ArgumentOutOfRangeException(nameof(NumberOfLines), "Must be non-negastive");
    public int ErrorCount { get; }
        = ErrorCount >= 0 ? ErrorCount : throw new ArgumentOutOfRangeException(nameof(ErrorCount), "Must be non-negastive");
    public float ElapsedTIme { get; }
        = ElapsedTIme >= 0 ? ElapsedTIme : throw new ArgumentOutOfRangeException(nameof(ElapsedTIme), "Must be non-negastive");
}
