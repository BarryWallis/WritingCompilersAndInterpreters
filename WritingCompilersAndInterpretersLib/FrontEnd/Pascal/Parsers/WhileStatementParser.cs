using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

/// <summary>
/// Parse a WHILE statement.
/// </summary>
public class WhileStatementParser : StatementParser
{
    private readonly static ISet<PascalTokenType> _doTokenTypes;

    static WhileStatementParser()
    {
        _doTokenTypes = new HashSet<PascalTokenType>(StatementStartTokenTypes);
        _ = _doTokenTypes.Add(PascalTokenType.Do);
        _doTokenTypes.UnionWith(StatementFollowTokenTypes);
    }
    /// <summary>
    /// Parse a WHILE statement.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    public WhileStatementParser(StatementParser parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override IIntermediateCodeNode Parse(Token token)
    {
        token = GetNextToken();
        IIntermediateCodeNode loopNode 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Loop);
        IIntermediateCodeNode breakNode 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Test);
        IIntermediateCodeNode notNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Not);

        _ = loopNode.AddChild(breakNode);
        _ = breakNode.AddChild(notNode);

        ExpressionParser expressionParser = new(this);
        _ = notNode.AddChild(expressionParser.Parse(token));

        token = Synchronize(_doTokenTypes);
        if (token.TokenType == PascalTokenType.Do)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingDo, this);
        }

        StatementParser statementParser = new(this);
        _ = loopNode.AddChild(statementParser.Parse(token));

        return loopNode;
    }
}