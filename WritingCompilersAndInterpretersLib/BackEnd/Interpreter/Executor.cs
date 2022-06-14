using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// The exeutor for an interpreter back end.
/// </summary>
public class Executor : BackEnd
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly Executor? _parent;
#pragma warning restore IDE0052 // Remove unread private members

    protected static readonly RuntimeErrorHandler ErrorHandler = new();

    /// <value>Number of intstructions executed.</value>
    protected static int ExecutionCount { get; set; } = 0;

    /// <summary>
    /// Create a new executor.
    /// </summary>
    public Executor() => _parent = null;

    /// <summary>
    /// Create a new executor with the given parent.
    /// </summary>
    /// <param name="parent">The parent of this eecutor.</param>
    public Executor(Executor parent)
    {
        parent._observers.ToList().ForEach(o => _observers.Add(o));
        _parent = parent;
    }

    /// <inheritdoc/>
    public override void Process(IIntermediateCode intermediateCode, ISymbolTableStack symbolTableStack)
    {
        SymbolTableStack = symbolTableStack;
        IntermediateCode = intermediateCode;

        long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Debug.Assert(IntermediateCode.Root is not null);
        IIntermediateCodeNode rootNode = IntermediateCode.Root;
        StatementExecutor statementExecutor = new(this);
        _ = statementExecutor.Execute(rootNode);

        long endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        float elapsedTime = (endTime - startTime) / 1000f;
        int runtimeErrors = RuntimeErrorHandler.ErrorCount;

        SendMessage(new InterpreterSummaryMessage(ExecutionCount, runtimeErrors, elapsedTime));
    }
}
