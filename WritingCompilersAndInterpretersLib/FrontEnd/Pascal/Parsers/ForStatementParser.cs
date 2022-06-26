using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

/// <summary>
/// Parse a FOR statement.
/// </summary>
public class ForStatementParser : StatementParser
{
    private static readonly ISet<PascalTokenType> _toDowntoTokenTypes;
    private static readonly ISet<PascalTokenType> _doTokenTypes;

    static ForStatementParser()
    {
        _toDowntoTokenTypes = new HashSet<PascalTokenType>(ExpressionParser._expressionStartTokenTypes);
        _ = _toDowntoTokenTypes.Add(PascalTokenType.To);
        _ = _toDowntoTokenTypes.Add(PascalTokenType.Downto);
        _toDowntoTokenTypes.UnionWith(StatementFollowTokenTypes);

        _doTokenTypes = new HashSet<PascalTokenType>(StatementStartTokenTypes);
        _ = _doTokenTypes.Add(PascalTokenType.Do);
        _doTokenTypes.UnionWith(StatementFollowTokenTypes);
    }

    /// <summary>
    /// Create a FOR statement parser.
    /// </summary>
    /// <param name="parent"></param>
    public ForStatementParser(StatementParser parent) : base(parent)
    {
    }

    /// <inheritdoc/>
    public override IIntermediateCodeNode Parse(Token token)
    {
        token = GetNextToken();
        Token targetToken = token;
        IIntermediateCodeNode compoundNode
           = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Compound);
        IIntermediateCodeNode loopNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Loop);
        IIntermediateCodeNode testNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Test);
        AssignmentStatementParser assignmentStatementParser = new(this);
        IIntermediateCodeNode initialAssignmentNode = assignmentStatementParser.Parse(token);
        SetLineNumber(initialAssignmentNode, targetToken);

        _ = compoundNode.AddChild(initialAssignmentNode);
        _ = compoundNode.AddChild(loopNode);

        token = Synchronize(_toDowntoTokenTypes);
        Debug.Assert(token.TokenType is not null);
        PascalTokenType direction = token.TokenType;
        if ((direction == PascalTokenType.To) || (direction == PascalTokenType.Downto))
        {
            token = GetNextToken();
        }
        else
        {
            direction = PascalTokenType.To;
            errorHandler.Flag(token, PascalErrorCode.MissingToDownto, this);
        }
        IntermediateCodeNodeType directionNodeType
            = direction == PascalTokenType.To ? IntermediateCodeNodeType.Gt
                                              : IntermediateCodeNodeType.Lt;
        IIntermediateCodeNode relationalOperatorNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(directionNodeType);

        IIntermediateCodeNode controlVaribleNode = initialAssignmentNode.Children.First();
        _ = relationalOperatorNode.AddChild(controlVaribleNode.Copy());
        ExpressionParser expressionParser = new(this);
        _ = relationalOperatorNode.AddChild(expressionParser.Parse(token));
        _ = testNode.AddChild(relationalOperatorNode);
        _ = loopNode.AddChild(testNode);

        token = SynchronizeDo();

        StatementParser statementParser = new(this);
        _ = loopNode.AddChild(statementParser.Parse(token));
        IIntermediateCodeNode nextAssignmentNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Assign);
        _ = nextAssignmentNode.AddChild(controlVaribleNode.Copy());
        IntermediateCodeNodeType arithmeticDirectionNodeType
            = direction == PascalTokenType.To ? IntermediateCodeNodeType.Add
                                              : IntermediateCodeNodeType.Subtract;
        IIntermediateCodeNode arithmeticOperatorNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(arithmeticDirectionNodeType);
        _ = arithmeticOperatorNode.AddChild(controlVaribleNode.Copy());
        IIntermediateCodeNode oneNode
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.IntegerConstant);
        oneNode.SetAttribute(IntermediateCodeKey.Value, 1);
        _ = arithmeticOperatorNode.AddChild(oneNode);
        _ = nextAssignmentNode.AddChild(arithmeticOperatorNode);
        _ = loopNode.AddChild(nextAssignmentNode);

        SetLineNumber(nextAssignmentNode, targetToken);
        return compoundNode;
    }

    private Token SynchronizeDo()
    {
        Token token = Synchronize(_doTokenTypes);
        if (token.TokenType == PascalTokenType.Do)
        {
            token = GetNextToken();
        }
        else
        {
            errorHandler.Flag(token, PascalErrorCode.MissingDo, this);
        }

        return token;
    }
}