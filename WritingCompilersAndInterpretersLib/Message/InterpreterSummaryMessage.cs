using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingCompilersAndInterpretersLib.Message;

public record InterpreterSummaryMessage(int ExecutionCount, int RuntimeErrors, float ElapsedTime) : Message
{
    public int ExecutionCount { get; }
        = ExecutionCount >= 0 ? ExecutionCount
                              : throw new ArgumentOutOfRangeException(nameof(ExecutionCount), "Must be non-negative");

    public int RuntimeErrors { get; }
        = RuntimeErrors >= 0 ? RuntimeErrors
                              : throw new ArgumentOutOfRangeException(nameof(RuntimeErrors), "Must be non-negative");

    public float ElapsedTime { get; }
        = ElapsedTime >= 0.0f ? ElapsedTime
                              : throw new ArgumentOutOfRangeException(nameof(ElapsedTime), "Must be non-negative");
}
