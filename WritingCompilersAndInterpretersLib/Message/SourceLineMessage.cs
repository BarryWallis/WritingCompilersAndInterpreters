using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingCompilersAndInterpretersLib.Message;

public record SourceLineMessage(int LineNumber, string Line) : Message
{
    public int LineNumber { get; init; } 
        = LineNumber > 0 ? LineNumber : throw new ArgumentOutOfRangeException(nameof(LineNumber), "Must be positive");
}
