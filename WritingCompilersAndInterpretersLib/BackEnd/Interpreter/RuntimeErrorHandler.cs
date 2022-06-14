using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Runtime error handler for the backend interpreter.
/// </summary>
public class RuntimeErrorHandler
{
    private const int _maxErrors = 5;

    public static int ErrorCount { get; private set; } = 0;

#pragma warning disable CA1822 // Mark members as static
    public void Flag(IIntermediateCodeNode node, RuntimeErrorCode errorCode, BackEnd backEnd)
#pragma warning restore CA1822 // Mark members as static
    {
        //string? lineNumber = null;
        IIntermediateCodeNode? currentNode = node;
        while ((currentNode is not null) && (currentNode.GetAttribute(IntermediateCodeKey.Line) == null))
        {
            currentNode = currentNode.Parent;
        }

        Debug.Assert(errorCode.ToString() is not null);
        Debug.Assert(currentNode is not null);
        Debug.Assert(currentNode.GetAttribute(IntermediateCodeKey.Line) is not null);
        backEnd.SendMessage(new RuntimeErrorMessage(errorCode.ToString()!,
                                                    (int)currentNode.GetAttribute(IntermediateCodeKey.Line)!));
        if (++ErrorCount > _maxErrors)
        {
            Console.Error.WriteLine("*** ABORTED AFTER TOO MANY RUNTIME ERRORS");
            Environment.Exit(-1);
        }
    }
}
