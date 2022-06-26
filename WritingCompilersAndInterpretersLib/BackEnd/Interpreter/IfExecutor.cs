using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

public class IfExecutor : StatementExecutor
{
    public IfExecutor(Executor parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override object? Execute(IIntermediateCodeNode node)
    {
        IReadOnlyCollection<IIntermediateCodeNode> children = node.Children;
        IIntermediateCodeNode expressionNode = children.First();
        IIntermediateCodeNode thenStatementNode = children.ToList()[1];
        Debug.Assert(children.Count <= 3);
        IIntermediateCodeNode? elseStatementNode = children.Count == 3 ? children.Last() : null;

        ExpressionExecutor expressionExecutor = new(this);
        StatementExecutor statementExecutor = new(this);

        bool b = (bool)expressionExecutor.Execute(expressionNode);
        if (b)
        {
            _ = statementExecutor.Execute(thenStatementNode);
        }
        else if (elseStatementNode is not null)
        {
            _ = statementExecutor.Execute(elseStatementNode);
        }

        ExecutionCount += 1;
        return null;
    }
}