using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute a LOOP statement.
/// </summary>
public class LoopExecutor : StatementExecutor
{
    /// <summary>
    /// Create a LOOP statement executor.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    public LoopExecutor(Executor parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override object? Execute(IIntermediateCodeNode node)
    {
        bool exitLoop = false;
        IIntermediateCodeNode? expressionNode = null;
        IReadOnlyCollection<IIntermediateCodeNode> loopChildren = node.Children;
        ExpressionExecutor expressionExecutor = new(this);
        StatementExecutor statementExecutor = new(this);
        while (!exitLoop)
        {
            ExecutionCount += 1;
            foreach (IIntermediateCodeNode child in loopChildren)
            {
                IntermediateCodeNodeType childType = child.NodeType;
                if (childType == IntermediateCodeNodeType.Test)
                {
                    if (expressionNode is null)
                    {
                        expressionNode = child.Children.First();
                    }
                    exitLoop = (bool)expressionExecutor.Execute(expressionNode);
                }
                else
                {
                    _ = statementExecutor.Execute(child);
                }

                if (exitLoop)
                {
                    break;
                }
            }
        }

        return null;
    }
}