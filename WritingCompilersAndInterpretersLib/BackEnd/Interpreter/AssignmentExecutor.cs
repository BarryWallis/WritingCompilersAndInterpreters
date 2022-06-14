using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute an assignment statement.
/// </summary>
public class AssignmentExecutor : StatementExecutor
{
    /// <summary>
    /// Create an assign statement executor.
    /// </summary>
    /// <param name="parent">The parent of the assignment statement.</param>
    public AssignmentExecutor(StatementExecutor parent) : base(parent)
    {

    }

    /// <summary>
    /// Create an assign statement executor.
    /// </summary>
    /// <param name="node">The root node of the assignment statement.</param>
    /// <returns>Always returns <see langword="null"/>.</returns>
    public override object? Execute(IIntermediateCodeNode node)
    {
        if (node.Children.Count != 2)
        {
            throw new ArgumentException("Assignment statement must have exactly two children", nameof(node));
        }

        IIntermediateCodeNode variableNode = node.Children.First();
        IIntermediateCodeNode expressionNode = node.Children.Last();
        ExpressionExecutor expressionExecutor = new(this);
        object? value = expressionExecutor.Execute(expressionNode);
        Debug.Assert(variableNode.GetAttribute(IntermediateCodeKey.Id) as ISymbolTableEntry is not null);
        ISymbolTableEntry variableId
            = (variableNode.GetAttribute(IntermediateCodeKey.Id) as ISymbolTableEntry)!;
        Debug.Assert(value is not null);
        variableId.SetAttribute(Intermediate.SymbolTableImplementation.SymbolTableKey.DataValue, value);
        SendMessage(node, variableId.Name, value);
        ExecutionCount += 1;
        return null;
    }

    /// <summary>
    /// Send a message about the assignment operation.
    /// </summary>
    /// <param name="node">The assignment expression node.</param>
    /// <param name="variableName">The name of the taarget variable.</param>
    /// <param name="value">The value of the expression.</param>
    private void SendMessage(IIntermediateCodeNode node, string variableName, object value)
    {
        int? lineNumber
            = node.GetAttribute(IntermediateCodeKey.Line) as int?;
        if (lineNumber is not null)
        {
            SendMessage(new AssignmentMessage((int)lineNumber, variableName, value));
        }
    }
}