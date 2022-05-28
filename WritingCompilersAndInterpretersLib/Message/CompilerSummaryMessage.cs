namespace WritingCompilersAndInterpretersLib.Message;

public record CompilerSummaryMessage(int InstructionCount, float ElapsedTime) : Message
{
    public int InstructionCount { get; }
        = InstructionCount >= 0 ? InstructionCount
                                : throw new ArgumentOutOfRangeException(nameof(InstructionCount), "Must be non-negative");

    public float ElpasedTime { get; }
        = ElapsedTime >= 0.0f ? ElapsedTime
                           : throw new ArgumentOutOfRangeException(nameof(ElapsedTime), "Must be non-negative");
}
