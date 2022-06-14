using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter;

/// <summary>
/// Execute an expression.
/// </summary>
public class ExpressionExecutor : StatementExecutor
{
    private readonly HashSet<IntermediateCodeNodeType> _arithmeticOperators = new()
    {
        IntermediateCodeNodeType.Add,
        IntermediateCodeNodeType.Subtract,
        IntermediateCodeNodeType.Multiply,
        IntermediateCodeNodeType.FloatDivide,
        IntermediateCodeNodeType.IntegerDivide,
    };

    /// <summary>
    /// Create a new expression executor.
    /// </summary>
    /// <param name="parent">The parent of the assignment statement.</param>
    public ExpressionExecutor(StatementExecutor parent) : base(parent)
    {
    }

    /// <summary>
    /// Execute an expression.
    /// </summary>
    /// <param name="node">The root intermedite node of the compound statement.</param>
    /// <returns>The computed value of the expression.</returns>
    public override object Execute(IIntermediateCodeNode node)
    {
        switch (node.NodeType)
        {
            case IntermediateCodeNodeType.Variable:
                return ProcessVariable(node);
            case IntermediateCodeNodeType.Not:
                Debug.Assert(node.Children.Count == 1);
                return !(bool)Execute(node.Children.First());
            case IntermediateCodeNodeType.Negate:
                return ProcessNegate(node);
            case IntermediateCodeNodeType.IntegerConstant:
                Debug.Assert(node.GetAttribute(IntermediateCodeKey.Value) is not null);
                return (int)node.GetAttribute(IntermediateCodeKey.Value)!;
            case IntermediateCodeNodeType.RealConstant:
                Debug.Assert(node.GetAttribute(IntermediateCodeKey.Value) is not null);
                return (float)node.GetAttribute(IntermediateCodeKey.Value)!;
            case IntermediateCodeNodeType.StringConstant:
                Debug.Assert(node.GetAttribute(IntermediateCodeKey.Value) as string is not null);
                return (node.GetAttribute(IntermediateCodeKey.Value) as string)!;
            default:
                return ExecuteBinaryOperator(node, node.NodeType);
        }
    }

    /// <summary>
    /// Execute a binary operator.
    /// </summary>
    /// <param name="node">The root node of the expression.</param>
    /// <param name="nodeType">The node type.</param>
    /// <returns>THe computed value of the expression.</returns>
    private object ExecuteBinaryOperator(IIntermediateCodeNode node, IntermediateCodeNodeType nodeType)
    {
        if (node.Children.Count != 2)
        {
            throw new ArgumentException("Binary node must have exactly two operands", nameof(node));
        }

        object operand1 = Execute(node.Children.First());
        object operand2 = Execute(node.Children.Last());
        bool integerMode = operand1 is int && operand2 is int;
        if (_arithmeticOperators.Contains(nodeType))
        {
            if (integerMode)
            {
                int value1 = (int)operand1;
                int value2 = (int)operand2;
                switch (nodeType)
                {
                    case IntermediateCodeNodeType.Add:
                        return value1 + value2;
                    case IntermediateCodeNodeType.Subtract:
                        return value1 - value2;
                    case IntermediateCodeNodeType.Multiply:
                        return value1 * value2;
                    case IntermediateCodeNodeType.IntegerDivide:
                        if (value2 == 0)
                        {
                            ErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
                            return 0;
                        }
                        else
                        {
                            return value1 / value2;
                        }
                    case IntermediateCodeNodeType.FloatDivide:
                        if (value2 == 0)
                        {
                            ErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
                            return 0;
                        }
                        else
                        {
                            return value1 / (float)value2;
                        }
                    case IntermediateCodeNodeType.Mod:
                        if (value2 == 0)
                        {
                            ErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
                            return 0;
                        }
                        else
                        {
                            return value1 % value2;
                        }
                }
            }
            else
            {
#pragma warning disable IDE0038 // Use pattern matching
                float value1 = operand1 is int ? (int)operand1 : (float)operand1;
                float value2 = operand2 is int ? (int)operand2 : (float)operand2;
#pragma warning restore IDE0038 // Use pattern matching
                switch (nodeType)
                {
                    case IntermediateCodeNodeType.Add:
                        return value1 + value2;
                    case IntermediateCodeNodeType.Subtract:
                        return value1 - value2;

                    case IntermediateCodeNodeType.Multiply:
                        return value1 * value2;
                    case IntermediateCodeNodeType.FloatDivide:
                        if (value2 == 0.0f)
                        {
                            ErrorHandler.Flag(node, RuntimeErrorCode.DivisionByZero, this);
                            return 0;
                        }
                        else
                        {
                            return value1 / value2;
                        }
                }
            }
        }
        else if (nodeType is IntermediateCodeNodeType.And or IntermediateCodeNodeType.Or)
        {
            bool value1 = (bool)operand1;
            bool value2 = (bool)operand2;
            switch (nodeType)
            {
                case IntermediateCodeNodeType.And:
                    return value1 && value2;
                case IntermediateCodeNodeType.Or:
                    return value1 || value2;
            }
        }
        else if (integerMode)
        {
            int value1 = (int)operand1;
            int value2 = (int)operand2;
            switch (nodeType)
            {
                case IntermediateCodeNodeType.Eq:
                    return value1 == value2;
                case IntermediateCodeNodeType.Ne:
                    return value1 != value2;
                case IntermediateCodeNodeType.Lt:
                    return value1 < value2;
                case IntermediateCodeNodeType.Le:
                    return value1 <= value2;
                case IntermediateCodeNodeType.Gt:
                    return value1 > value2;
                case IntermediateCodeNodeType.Ge:
                    return value1 >= value2;
            }
        }
        else
        {
#pragma warning disable IDE0038 // Use pattern matching
            float value1 = operand1 is int ? (int)operand1 : (float)operand1;
            float value2 = operand2 is int ? (int)operand2 : (float)operand2;
#pragma warning restore IDE0038 // Use pattern matching
            switch (nodeType)
            {
                case IntermediateCodeNodeType.Eq:
                    return value1 == value2;
                case IntermediateCodeNodeType.Ne:
                    return value1 != value2;
                case IntermediateCodeNodeType.Lt:
                    return value1 < value2;
                case IntermediateCodeNodeType.Le:
                    return value1 <= value2;
                case IntermediateCodeNodeType.Gt:
                    return value1 > value2;
                case IntermediateCodeNodeType.Ge:
                    return value1 >= value2;
            }
        }

        Debug.Fail($"Unexpected IntermediateCodeNodeType: {node.NodeType}");
        return 0;
    }

    private static object ProcessVariable(IIntermediateCodeNode node)
    {
        Debug.Assert(node.GetAttribute(IntermediateCodeKey.Id) as ISymbolTableEntry is not null);
        ISymbolTableEntry symbolTableEntry = (node.GetAttribute(IntermediateCodeKey.Id) as ISymbolTableEntry)!;
        Debug.Assert(symbolTableEntry.GetAttribute(SymbolTableKey.DataValue) is not null);
        return symbolTableEntry.GetAttribute(SymbolTableKey.DataValue)!;
    }

    private object ProcessNegate(IIntermediateCodeNode node)
    {
        Debug.Assert(node.Children.Count == 1);
        object value = Execute(node.Children.First());
        if (value is int intValue)
        {
            return -intValue;
        }
        else if (value is float floatValue)
        {
            return -floatValue;
        }
        else
        {
            Debug.Fail($"Unexpected IntermediateCodeNodeType to negate: {node.NodeType}");
            return 0;
        }
    }
}