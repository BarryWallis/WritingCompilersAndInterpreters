using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute a compund statement.
/// </summary>
public class CompoundExecutor : StatementExecutor
{
    /// <summary>
    /// Create a compound statement.
    /// </summary>
    /// <param name="parent">The parent of the assignment statement.</param>
    public CompoundExecutor(StatementExecutor parent) : base(parent)
    {

    }

    /// <summary>
    /// Execute a compund statement.
    /// </summary>
    /// <param name="node">The root of the compound statement.</param>
    /// <returns>Always returns <see langword="null"/>.</returns>
    public override object? Execute(IIntermediateCodeNode node)
    {
        StatementExecutor statementExecutor = new(this);
        node.Children.ToList().ForEach(child => statementExecutor.Execute(child));
        return null;
    }
}