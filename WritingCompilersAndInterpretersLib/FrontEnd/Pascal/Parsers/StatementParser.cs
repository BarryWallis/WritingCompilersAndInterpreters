using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class StatementParser : PascalParserTopDown
{
    protected static readonly ISet<PascalTokenType> StatementStartTokenTypes = new HashSet<PascalTokenType>()
    {
        PascalTokenType.Begin,
        PascalTokenType.Case,
        PascalTokenType.For,
        PascalTokenType.If,
        PascalTokenType.Repeat,
        PascalTokenType.While,
        PascalTokenType.Identifier,
        PascalTokenType.Semicolon,
    };
    protected static readonly ISet<PascalTokenType> StatementFollowTokenTypes = new HashSet<PascalTokenType>()
    {
        PascalTokenType.Semicolon,
        PascalTokenType.End,
        PascalTokenType.Else,
        PascalTokenType.Until,
        PascalTokenType.Dot,
    };

    /// <summary>
    /// Create new statement parser.
    /// </summary>
    /// <param name="parent">The parent parser.</param>
    public StatementParser(PascalParserTopDown parent) : base(parent)
    {

    }

    /// <summary>
    /// Parse a statement.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node.</returns>
    public virtual IIntermediateCodeNode? Parse(Token token)
    {
        IIntermediateCodeNode statementNode;
        if (token.TokenType == PascalTokenType.Begin)
        {
            CompoundStatementParser compoundStatementParser = new(this);
            statementNode = compoundStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.Identifier)
        {
            AssignmentStatementParser assignmentStatementParser = new(this);
            statementNode = assignmentStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.Repeat)
        {
            RepeatStatementParser repeatStatementParser = new(this);
            statementNode = repeatStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.While)
        {
            WhileStatementParser whileStatementParser = new(this);
            statementNode = whileStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.For)
        {
            ForStatementParser forStatementParser = new(this);
            statementNode = forStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.If)
        {
            IfStatementParser ifStatementParser = new(this);
            statementNode = ifStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.Case)
        {
            CaseStatementParser caseStatementParser = new(this);
            statementNode = caseStatementParser.Parse(token);
        }
        else
        {
            statementNode = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.NoOp);
        }

        SetLineNumber(statementNode, token);

        return statementNode;
    }

    protected static void SetLineNumber(IIntermediateCodeNode node, Token token)
    {
        if (node is not null)
        {
            node.SetAttribute(IntermediateCodeKey.Line, token.LineNumber);
        }
    }

    /// <summary>
    /// Parse a statement list.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="parentNode">The parent node of the statement list.</param>
    /// <param name="terminator">The token type of the node that terminates the list.</param>
    /// <param name="errorCode">The error code if the terminator is missing.</param>
    public void ParseList(Token token, IIntermediateCodeNode parentNode, PascalTokenType terminator,
                             PascalErrorCode errorCode)
    {
        ISet<PascalTokenType> terminatorTokenTypes = new HashSet<PascalTokenType>(StatementStartTokenTypes);
        _ = terminatorTokenTypes.Add(terminator);

        while (token is not EndOfFileToken && token.TokenType != terminator)
        {
            IIntermediateCodeNode? statementNode = Parse(token);
            _ = parentNode.AddChild(statementNode);

            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;
            Debug.Assert(token.TokenType is not null);
            TokenType tokenType = token.TokenType;
            if (tokenType == PascalTokenType.Semicolon)
            {
                _ = GetNextToken();
            }
            else if (StatementStartTokenTypes.Contains(tokenType))
            {
                errorHandler.Flag(token, PascalErrorCode.MissingSemicolon, this);
            }

            token = Synchronize(terminatorTokenTypes);
        }

        if (token.TokenType == terminator)
        {
            _ = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, errorCode, this);
        }
    }
}