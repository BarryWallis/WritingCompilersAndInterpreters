using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

/// <summary>
/// Parse a REPEAT statement.
/// </summary>
public class RepeatStatementParser : StatementParser
{
    /// <summary>
    /// Create a REPEAT statement parser.
    /// </summary>
    /// <param name="parent"></param>
    public RepeatStatementParser(PascalParserTopDown parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override IIntermediateCodeNode Parse(Token token)
    {
        token = GetNextToken();
        IIntermediateCodeNode loopNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Loop);
        IIntermediateCodeNode testNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Test);
        StatementParser statementParser = new(this);
        statementParser.ParseList(token, loopNode, PascalTokenType.Until, PascalErrorCode.MissingUntil);
        Debug.Assert(CurrentToken is not null);
        token = CurrentToken;
        ExpressionParser expressionParser = new(this);
        _ = testNode.AddChild(expressionParser.Parse(token));
        _ = loopNode.AddChild(testNode);
        return loopNode;
    }
}