using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class StatementParser : PascalParserTopDown
{
    /// <summary>
    /// Create new statement parser.
    /// </summary>
    /// <param name="parent">The parent parser.</param>
    public StatementParser(PascalParserTopDown parent) : base(parent)
    {

    }

    public virtual IIntermediateCodeNode Parse(Token token)
    {
        IIntermediateCodeNode statementNode;
        if (token.TokenType == PascalTokenType.Begin)
        {
            CompountStatementParser compoundStatementParser = new(this);
            statementNode = compoundStatementParser.Parse(token);
        }
        else if (token.TokenType == PascalTokenType.Identifier)
        {
            AssignmentStatementParser assignmentStatementParser = new(this);
            statementNode = assignmentStatementParser.Parse(token);
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
        while (token is not EndOfFileToken && token.TokenType != terminator)
        {
            IIntermediateCodeNode statementNode = Parse(token);
            _ = parentNode.AddChild(statementNode);

            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;
            Debug.Assert(token.TokenType is not null);
            TokenType tokenType = token.TokenType;
            if (tokenType == PascalTokenType.Semicolon)
            {
                token = GetNextToken();
            }
            else if (tokenType == PascalTokenType.Identifier)
            {
                errorHandler.Flag(token, PascalErrorCode.MissingSemicolon, this);
            }
            else if (tokenType != terminator)
            {
                errorHandler.Flag(token, PascalErrorCode.UnexpectedToken, this);
                token = GetNextToken();
            }
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