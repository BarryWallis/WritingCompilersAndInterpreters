using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class ExpressionParser : StatementParser
{
    internal static IEnumerable<PascalTokenType> _expressionStartTokenTypes = new HashSet<PascalTokenType>()
    {
        PascalTokenType.Plus,
        PascalTokenType.Minus,
        PascalTokenType.Identifier,
        PascalTokenType.Integer,
        PascalTokenType.Real,
        PascalTokenType.String,
        PascalTokenType.Not,
        PascalTokenType.LeftParen,
    };

    private readonly Dictionary<PascalTokenType, IntermediateCodeNodeType> _relationalOperators = new()
    {
        [PascalTokenType.Equal] = IntermediateCodeNodeType.Eq,
        [PascalTokenType.NotEqual] = IntermediateCodeNodeType.Ne,
        [PascalTokenType.LessThan] = IntermediateCodeNodeType.Lt,
        [PascalTokenType.LessEquals] = IntermediateCodeNodeType.Le,
        [PascalTokenType.GreaterThan] = IntermediateCodeNodeType.Gt,
        [PascalTokenType.GreaterEquals] = IntermediateCodeNodeType.Ge,
    };

    private readonly Dictionary<PascalTokenType, IntermediateCodeNodeType> _additiveOperators = new()
    {
        [PascalTokenType.Plus] = IntermediateCodeNodeType.Add,
        [PascalTokenType.Minus] = IntermediateCodeNodeType.Subtract,
        [PascalTokenType.Or] = IntermediateCodeNodeType.Or,
    };

    private readonly Dictionary<PascalTokenType, IntermediateCodeNodeType> _multiplicativeOperators = new()
    {
        [PascalTokenType.Star] = IntermediateCodeNodeType.Multiply,
        [PascalTokenType.Slash] = IntermediateCodeNodeType.FloatDivide,
        [PascalTokenType.Div] = IntermediateCodeNodeType.IntegerDivide,
        [PascalTokenType.Mod] = IntermediateCodeNodeType.Mod,
        [PascalTokenType.And] = IntermediateCodeNodeType.And,
    };
    public ExpressionParser(PascalParserTopDown parent) : base(parent)
    {
    }

    /// <summary>
    /// Parse an expression.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    public override IIntermediateCodeNode? Parse(Token token) => ParseExpression(token);

    /// <summary>
    /// Parse an expression.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    private IIntermediateCodeNode? ParseExpression(Token token)
    {
        IIntermediateCodeNode? rootNode = ParseSimpleExpression(token);
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        Debug.Assert(token.TokenType is not null);
        PascalTokenType tokenType = token.TokenType;

        if (_relationalOperators.ContainsKey(tokenType))
        {
            IntermediateCodeNodeType nodeType = _relationalOperators[tokenType];
            IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);
            _ = operatorNode.AddChild(rootNode);
            token = GetNextToken();
            IIntermediateCodeNode? childNode = ParseSimpleExpression(token);
            _ = operatorNode.AddChild(childNode);
            rootNode = operatorNode;
        }

        return rootNode;
    }

    /// <summary>
    /// Parse a simple expression.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    private IIntermediateCodeNode? ParseSimpleExpression(Token token)
    {
        PascalTokenType? signType = null;
        Debug.Assert(token.TokenType is not null);
        PascalTokenType tokenType = token.TokenType;
        if (tokenType == PascalTokenType.Plus || tokenType == PascalTokenType.Minus)
        {
            signType = tokenType;
            token = GetNextToken();
        }

        IIntermediateCodeNode? rootNode = ParseTerm(token);
        if (signType == PascalTokenType.Minus)
        {
            rootNode = ProcessMinus(rootNode);
        }

        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        Debug.Assert(token.TokenType is not null);
        tokenType = token.TokenType;
        while (_additiveOperators.ContainsKey(tokenType!))
        {
            ProcessAdditiveOperator(ref tokenType, ref rootNode);
        }

        return rootNode;
    }

    private void ProcessAdditiveOperator(ref PascalTokenType tokenType, ref IIntermediateCodeNode? rootNode)
    {
        Token token;
        IntermediateCodeNodeType nodeType = _additiveOperators[tokenType!];
        IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);
        _ = operatorNode.AddChild(rootNode);
        token = GetNextToken();
        IIntermediateCodeNode? childNode = ParseTerm(token);
        _ = operatorNode.AddChild(childNode);
        rootNode = operatorNode;
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        Debug.Assert(token.TokenType is not null);
        tokenType = token.TokenType;
    }

    private static IIntermediateCodeNode ProcessMinus(IIntermediateCodeNode? rootNode)
    {
        IIntermediateCodeNode negateNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Negate);
        _ = negateNode.AddChild(rootNode);
        rootNode = negateNode;
        return rootNode;
    }

    /// <summary>
    /// Parse a term.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    private IIntermediateCodeNode? ParseTerm(Token token)
    {
        IIntermediateCodeNode? rootNode = ParseFactor(token);
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        Debug.Assert(token.TokenType is not null);
        PascalTokenType tokenType = token.TokenType;
        while (_multiplicativeOperators.ContainsKey(tokenType!))
        {
            IntermediateCodeNodeType nodeType = _multiplicativeOperators[tokenType!];
            IIntermediateCodeNode operatorNode = IntermediateCodeFactory.CreateIntermediateCodeNode(nodeType);
            _ = operatorNode.AddChild(rootNode);
            token = GetNextToken();
            IIntermediateCodeNode? childNode = ParseFactor(token);
            _ = operatorNode.AddChild(childNode);
            rootNode = operatorNode;
            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;
            Debug.Assert(token.TokenType is not null);
            tokenType = token.TokenType;
        }

        return rootNode;
    }

    /// <summary>
    /// Parse a factor.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    private IIntermediateCodeNode? ParseFactor(Token token)
    {
        Debug.Assert(token.TokenType is not null);
        PascalTokenType tokenType = token.TokenType;
        IIntermediateCodeNode? rootNode = null;
        if (tokenType == PascalTokenType.Identifier)
        {
            rootNode = ProcessIdentifier(ref token);
        }
        else if (tokenType == PascalTokenType.Integer)
        {
            rootNode = ProcessInteger(ref token);
        }
        else if (tokenType == PascalTokenType.Real)
        {
            rootNode = ProcessReal(ref token);
        }
        else if (tokenType == PascalTokenType.String)
        {
            rootNode = ProcessString(ref token);
        }
        else if (tokenType == PascalTokenType.Not)
        {
            ProcessNot(out token, out rootNode);
        }
        else if (tokenType == PascalTokenType.LeftParen)
        {
            ProcessLeftParen(out token, out rootNode);
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
        }

        return rootNode;
    }

    private void ProcessLeftParen(out Token token, out IIntermediateCodeNode? rootNode)
    {
        token = GetNextToken();
        rootNode = ParseExpression(token);
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        if (token.TokenType == PascalTokenType.RightParen)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingRightParen, this);
        }
    }

    private void ProcessNot(out Token token, out IIntermediateCodeNode? rootNode)
    {
        token = GetNextToken();
        rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Not);
        IIntermediateCodeNode? childNode = ParseFactor(token);
        _ = rootNode.AddChild(childNode);
    }

    private IIntermediateCodeNode ProcessString(ref Token token)
    {
        IIntermediateCodeNode? rootNode;
        string value = (token.Value as string)!;
        rootNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.StringConstant);
        rootNode.SetAttribute(IntermediateCodeKey.Value, value);
        token = GetNextToken();
        return rootNode;
    }

    private IIntermediateCodeNode ProcessReal(ref Token token)
    {
        IIntermediateCodeNode? rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.RealConstant);
        Debug.Assert(token.Value is not null);
        rootNode.SetAttribute(IntermediateCodeKey.Value, token.Value);
        token = GetNextToken();
        return rootNode;
    }

    private IIntermediateCodeNode ProcessInteger(ref Token token)
    {
        IIntermediateCodeNode? rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.IntegerConstant);
        Debug.Assert(token.Value is not null);
        rootNode.SetAttribute(IntermediateCodeKey.Value, token.Value);
        token = GetNextToken();
        return rootNode;
    }

    private IIntermediateCodeNode ProcessIdentifier(ref Token token)
    {
        IIntermediateCodeNode? rootNode;
        Debug.Assert(token.Text is not null);
        string name = token.Text.ToLowerInvariant();
        ISymbolTableEntry? id = SymbolTableStack.Lookup(name);
        if (id is null)
        {
            errorHandler.Flag(token, PascalErrorCode.IdentifierUndefined, this);
            id = SymbolTableStack.EnterLocal(name);
        }

        rootNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Variable);
        Debug.Assert(id is not null);
        rootNode.SetAttribute(IntermediateCodeKey.Id, id);
        id.AppendLineNumber(token.LineNumber);
        token = GetNextToken();
        return rootNode;
    }
}