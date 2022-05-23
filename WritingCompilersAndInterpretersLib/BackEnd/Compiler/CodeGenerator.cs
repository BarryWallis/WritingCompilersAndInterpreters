using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Compiler;

public class CodeGenerator : BackEnd
{
    /// <inheritdoc/>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable)
    {
        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        float elapsedTime = (endTime - startTime) / 1000f;
        int instructionCount = 0;
        SendMessage(new CompilerSummaryMessage(instructionCount, elapsedTime));
    }
}
