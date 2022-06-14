using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute a statement.
/// </summary>
public class StatementExecutor : Executor
{
    public StatementExecutor(Executor parent) : base(parent)
    {
    }

    /// <summary>
    /// Execute a statement.
    /// </summary>
    /// <param name="node">The root node of the statement.</param>
    /// <returns>Always returns <see langword="null"/>.</returns>
    public virtual object? Execute(IIntermediateCodeNode node)
    {
        SendSourceLineessage(node);
        IntermediateCodeNodeType nodeType = node.NodeType;
        switch (nodeType)
        {
            case IntermediateCodeNodeType.Compound:
                {
                    CompoundExecutor compoundExecutor = new(this);
                    return compoundExecutor.Execute(node);
                }
            case IntermediateCodeNodeType.Assign:
                {
                    AssignmentExecutor assignmentExecutor = new(this);
                    return assignmentExecutor.Execute(node);
                }
            case IntermediateCodeNodeType.NoOp:
                return null;
            default:
                ErrorHandler.Flag(node, RuntimeErrorCode.UnimplementedFeature, this);
                return null;
        }
    }

    /// <summary>
    /// Send a message about the current source line.
    /// </summary>
    /// <param name="node">The statement node.</param>
    private void SendSourceLineessage(IIntermediateCodeNode node)
    {
        int? lineNumber = node.GetAttribute(IntermediateCodeKey.Line) as int?;
        if (lineNumber is not null)
        {
            SendMessage(new SourceLineMessage((int)lineNumber, string.Empty));
        }
    }
}