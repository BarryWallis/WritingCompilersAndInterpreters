using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

/// <summary>
/// Parse a CASE statement.
/// </summary>
public class CaseStatementParser : StatementParser
{
    private static readonly ISet<PascalTokenType> _constantStartTokenTypes;
    private static readonly ISet<PascalTokenType> _ofTokenTypes;
    private static readonly ISet<PascalTokenType> _commaTokenTypes;

    static CaseStatementParser()
    {
        _constantStartTokenTypes = new HashSet<PascalTokenType>()
        {
            PascalTokenType.Identifier,
            PascalTokenType.Integer,
            PascalTokenType.Plus,
            PascalTokenType.Minus,
            PascalTokenType.String,
        };

        _ofTokenTypes = new HashSet<PascalTokenType>(_constantStartTokenTypes);
        _ = _ofTokenTypes.Add(PascalTokenType.Of);
        _ofTokenTypes.UnionWith(StatementFollowTokenTypes);

        _commaTokenTypes = new HashSet<PascalTokenType>(_constantStartTokenTypes);
        _ = _commaTokenTypes.Add(PascalTokenType.Comma);
        _ = _commaTokenTypes.Add(PascalTokenType.Colon);
        _commaTokenTypes.UnionWith(StatementStartTokenTypes);
        _commaTokenTypes.UnionWith(StatementFollowTokenTypes);
    }

    /// <summary>
    /// Create a new CASE statement parser with the given parent.
    /// </summary>
    /// <param name="parent">The parent of this parser.</param>
    public CaseStatementParser(StatementParser parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override IIntermediateCodeNode Parse(Token token)
    {
        token = GetNextToken();

        IIntermediateCodeNode selectNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Select);

        ExpressionParser expressionParser = new(this);
        _ = selectNode.AddChild(expressionParser.Parse(token));

        token = Synchronize(_ofTokenTypes);
        if (token.TokenType == PascalTokenType.Of)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingOf, this);
        }

        ISet<object> constantSet = new HashSet<object>();
        while (token is not EndOfFileToken && token.TokenType != PascalTokenType.End)
        {
            _ = selectNode.AddChild(ParseBranch(token, constantSet));
            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;
            Debug.Assert(token.TokenType is not null);
            TokenType tokenType = token.TokenType;

            if (tokenType == PascalTokenType.Semicolon)
            {
                token = GetNextToken();
            }
            else if (_constantStartTokenTypes.Contains(tokenType))
            {
                errorHandler.Flag(token, PascalErrorCode.MissingSemicolon, this);
            }
        }

        if (token.TokenType == PascalTokenType.End)
        {
            _ = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingEnd, this);
        }

        return selectNode;
    }

    /// <summary>
    /// Parse a CASE branch.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="constants">The set of CASE branch constants.</param>
    /// <returns>The root SelectBranch node of the subtree.</returns>
    private IIntermediateCodeNode ParseBranch(Token token, ISet<object> constants)
    {
        IIntermediateCodeNode branchNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.SelectBranch);
        IIntermediateCodeNode constantsNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.SelectConstants);
        _ = branchNode.AddChild(constantsNode);
        ParseConstantList(token, constantsNode, constants);
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        if (token.TokenType == PascalTokenType.Colon)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingColon, this);
        }

        StatementParser statementParser = new(this);
        _ = branchNode.AddChild(statementParser.Parse(token));

        return branchNode;
    }

    /// <summary>
    /// Parse a list of CASE branch constants."
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="constantsNode">The parent SelectConstants node.</param>
    /// <param name="constants">The set of CASE branch constants.</param>
    private void ParseConstantList(Token token, IIntermediateCodeNode constantsNode, ISet<object> constants)
    {
        Debug.Assert(token.TokenType is not null);
        while (_constantStartTokenTypes.Contains(token.TokenType!))
        {
            _ = constantsNode.AddChild(ParseConstant(constants));
            Debug.Assert(token.TokenType is not null);
            token = Synchronize(_commaTokenTypes);
            if (token.TokenType == PascalTokenType.Comma)
            {
                token = GetNextToken();
            }
            else if (_constantStartTokenTypes.Contains(token.TokenType!))
            {
                errorHandler.Flag(token, PascalErrorCode.MissingComma, this);
            }
        }
    }

    /// <summary>
    /// Parse a CASE branch constant.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="constants">The set of CASE branch constants.</param>
    /// <returns>The constant node.</returns>
    /// <exception cref="NotImplementedException"></exception>
    private IIntermediateCodeNode? ParseConstant(ISet<object> constants)
    {
        TokenType? sign = null;
        IIntermediateCodeNode? constantNode = null;

        Token token = Synchronize(_constantStartTokenTypes);
        Debug.Assert(token.TokenType is not null);
        TokenType tokenType = token.TokenType;

        if (tokenType == PascalTokenType.Plus || tokenType == PascalTokenType.Minus)
        {
            sign = tokenType;
            token = GetNextToken();
        }

        if (token.TokenType == PascalTokenType.Identifier)
        {
            constantNode = ParseIdentifierConstant(token, sign);
        }
        else if (token.TokenType == PascalTokenType.Integer)
        {
            Debug.Assert(token.Text is not null);
            constantNode = ParseIntegerConstant(token.Text, sign);
        }
        else if (token.TokenType == PascalTokenType.String)
        {
            Debug.Assert(token.Value is not null);
            Debug.Assert(token.Value is string);
            constantNode = ParseCharacterConstant(token, (token.Value as string)!, sign);
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.InvalidConstant, this);
        }

        if (constantNode is not null)
        {
            Debug.Assert(constantNode.GetAttribute(IntermediateCodeKey.Value) is not null);
            object value = constantNode.GetAttribute(IntermediateCodeKey.Value)!;
            if (constants.Contains(value))
            {
                errorHandler.Flag(token, PascalErrorCode.CaseConstantReused, this);
            }
            else
            {
                _ = constants.Add(value);
            }
        }

        _ = GetNextToken();
        return constantNode;
    }

    /// <summary>
    /// Parse a character CASE constant.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="value">The token value string.</param>
    /// <param name="sign">The sign, if any.</param>
    /// <returns>The constant node.</returns>
    private IIntermediateCodeNode? ParseCharacterConstant(Token token, string value, TokenType? sign)
    {
        IIntermediateCodeNode? constantNode = null;

        if (sign is not null)
        {
            errorHandler.Flag(token, PascalErrorCode.InvalidConstant, this);
        }
        else
        {
            if (value.Length == 1)
            {
                constantNode
                    = IntermediateCodeFactory
                        .CreateIntermediateCodeNode(IntermediateCodeNodeType.StringConstant);
                constantNode.SetAttribute(IntermediateCodeKey.Value, value);
            }
            else
            {
                errorHandler.Flag(token, PascalErrorCode.InvalidConstant, this);
            }
        }

        return constantNode;
    }

    /// <summary>
    /// Parse an integer CASE constant.
    /// </summary>
    /// <param name="value">The current token value string.</param>
    /// <param name="sign">The sign, if any.</param>
    /// <returns>The constant node.</returns>
    private static IIntermediateCodeNode? ParseIntegerConstant(string value, TokenType? sign)
    {
        IIntermediateCodeNode constantNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.IntegerConstant);
        int integerValue = int.Parse(value);

        if (sign == PascalTokenType.Minus)
        {
            integerValue = -integerValue;
        }

        constantNode.SetAttribute(IntermediateCodeKey.Value, integerValue);
        return constantNode;
    }

    /// <summary>
    /// Parse an identifier CASE constant.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="sign">The sign, if any.</param>
    /// <returns>The constant node.</returns>
    private IIntermediateCodeNode? ParseIdentifierConstant(Token token, TokenType? sign)
    {
        errorHandler.Flag(token, PascalErrorCode.InvalidConstant, this);
        return null;
    }
}