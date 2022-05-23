using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

public class Executor : BackEnd
{
    /// <inheritdoc/>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTable symbolTable)
    {
        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        float elapsedTime = (endTime - startTime) / 1000f;
        int executionCount = 0;
        int runtimeErrors = 0;

        SendMessage(new InterpreterSummaryMessage(executionCount, runtimeErrors, elapsedTime));
    }
}
