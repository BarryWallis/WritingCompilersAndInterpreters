using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class AssignmentStatementParser : StatementParser
{
    private static readonly ISet<PascalTokenType> _colonEqualsTokenTypes;

    static AssignmentStatementParser()
    {
        _colonEqualsTokenTypes = new HashSet<PascalTokenType>(ExpressionParser._expressionStartTokenTypes);
        _ = _colonEqualsTokenTypes.Add(PascalTokenType.ColonEquals);
        _colonEqualsTokenTypes.UnionWith(StatementFollowTokenTypes);
    }

    public AssignmentStatementParser(PascalParserTopDown parent) : base(parent)
    {
    }

    /// <summary>
    /// Parse an assignment statement.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the genersted parse tree.</returns>
    public override IIntermediateCodeNode Parse(Token token)
    {
        IIntermediateCodeNode assignNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Assign);
        Debug.Assert(token.Text is not null);
        string targetName = token.Text.ToLowerInvariant();
        ISymbolTableEntry? targetId = SymbolTableStack.Lookup(targetName);
        if (targetId is null)
        {
            targetId = SymbolTableStack.EnterLocal(targetName);
        }
        Debug.Assert(targetId is not null);
        targetId.AppendLineNumber(token.LineNumber);

        _ = GetNextToken();
        IIntermediateCodeNode variableNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Variable);
        variableNode.SetAttribute(IntermediateCodeKey.Id, targetId);
        _ = assignNode.AddChild(variableNode);

        token = Synchronize(_colonEqualsTokenTypes);
        if (token.TokenType == PascalTokenType.ColonEquals)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingColonEquals, this);
        }

        ExpressionParser expressionParser = new(this);
        _ = assignNode.AddChild(expressionParser.Parse(token));
        return assignNode;
    }
}