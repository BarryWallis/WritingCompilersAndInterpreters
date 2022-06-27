using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute SELECT statements.
/// </summary>
public class SelectExecutor : StatementExecutor
{
    private static readonly
        IDictionary<IIntermediateCodeNode, IDictionary<object, IIntermediateCodeNode>> _jumpCache
            = new Dictionary<IIntermediateCodeNode, IDictionary<object, IIntermediateCodeNode>>();

    /// <summary>
    /// Create a new SELECT statement executor.
    /// </summary>
    /// <param name="parent">The parent node of the statement.</param>
    public SelectExecutor(Executor parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override object? Execute(IIntermediateCodeNode node)
    {
        if (!_jumpCache.TryGetValue(node, out IDictionary<object, IIntermediateCodeNode>? jumpTable))
        {
            jumpTable = CreateJumpTable(node);
            _jumpCache[node] = jumpTable;
        }

        IReadOnlyCollection<IIntermediateCodeNode> selectChildren = node.Children;
        IIntermediateCodeNode expressionNode = selectChildren.First();

        ExpressionExecutor expressionExecutor = new(this);
        object selectValue = expressionExecutor.Execute(expressionNode);

        if (jumpTable.TryGetValue(selectValue, out IIntermediateCodeNode? statementNode))
        {
            StatementExecutor statementExecutor = new(this);
            _ = statementExecutor.Execute(statementNode);
        }
        //IIntermediateCodeNode? selectedBranchNode = SearchBranches(selectValue, selectChildren);
        //if (selectedBranchNode is not null)
        //{
        //    Debug.Assert(selectedBranchNode.Children.Count <= 2);
        //    IIntermediateCodeNode statementNode = selectedBranchNode.Children.Last();
        //    StatementExecutor statementExecutor = new(this);
        //    _ = statementExecutor.Execute(statementNode);
        //}

        ExecutionCount += 1;
        return null;
    }

    /// <summary>
    /// Create a jump table for a SELECT node.
    /// </summary>
    /// <param name="node">The SELECT node.</param>
    /// <returns>The jump table.</returns>
    private static IDictionary<object, IIntermediateCodeNode> CreateJumpTable(IIntermediateCodeNode node)
    {
        IDictionary<object, IIntermediateCodeNode> jumpTable = new Dictionary<object, IIntermediateCodeNode>();
        List<IIntermediateCodeNode> selectedChildren = node.Children.ToList();
        for (int i = 1; i < selectedChildren.Count; i++)
        {
            IIntermediateCodeNode branchNode = selectedChildren[i];
            Debug.Assert(branchNode.Children.Count == 2);
            IIntermediateCodeNode constantsNode = branchNode.Children.First();
            IIntermediateCodeNode statementNode = branchNode.Children.Last();
            foreach (IIntermediateCodeNode constantNode in constantsNode.Children)
            {
                Debug.Assert(constantNode.GetAttribute(IntermediateCodeKey.Value) is not null);
                object value = constantNode.GetAttribute(IntermediateCodeKey.Value)!;
                jumpTable[value] = statementNode;
            }
        }

        return jumpTable;
    }

    /// <summary>
    /// Serch the SELECT branches to find a match.
    /// </summary>
    /// <param name="selectValue">Th value to match.</param>
    /// <param name="selectChildren">The children of the select node.</param>
    /// <returns>The matched node.</returns>
    //private static IIntermediateCodeNode? SearchBranches(object selectValue, IReadOnlyCollection<IIntermediateCodeNode> selectChildren)
    //{
    //    List<IIntermediateCodeNode> selectChildrenList = selectChildren.ToList();
    //    for (int i = 1; i < selectChildren.Count; i++)
    //    {
    //        IIntermediateCodeNode branchNode = selectChildrenList[i];
    //        if (SearchConstants(selectValue, branchNode))
    //        {
    //            return branchNode;
    //        }
    //    }

    //    return null;
    //}

    /// <summary>
    /// Search the constants of a SELECT branch for a matching value.
    /// </summary>
    /// <param name="selectValue">The value to match.</param>
    /// <param name="branchNode">The SELECT_BRANCH node.</param>
    /// <returns>
    /// <see langword="true"/> if the value matches the branch node; <see langword="false"/> otherwise.
    /// </returns>
    //private static bool SearchConstants(object selectValue, IIntermediateCodeNode branchNode)
    //{
    //    bool integerMode = selectValue is int;

    //    IIntermediateCodeNode constantsNode = branchNode.Children.First();
    //    IReadOnlyCollection<IIntermediateCodeNode> constants = constantsNode.Children;
    //    if (integerMode)
    //    {
    //        foreach (IIntermediateCodeNode constantNode in constants)
    //        {
    //            Debug.Assert(constantNode.GetAttribute(IntermediateCodeKey.Value) is not null);
    //            int constant = (int)constantNode.GetAttribute(IntermediateCodeKey.Value)!;
    //            if ((int)selectValue == constant)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (IIntermediateCodeNode constantNode in constants)
    //        {
    //            Debug.Assert(constantNode.GetAttribute(IntermediateCodeKey.Value) is string);
    //            string constant = (constantNode.GetAttribute(IntermediateCodeKey.Value) as string)!;
    //            if ((selectValue as string) == constant)
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}
}