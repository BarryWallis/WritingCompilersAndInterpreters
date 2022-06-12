using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class CompountStatementParser : StatementParser
{
    public CompountStatementParser(PascalParserTopDown parent) : base(parent)
    {
    }

    /// <summary>
    /// Parse a compound statement.
    /// </summary>
    /// <param name="token">The initial token.</param>
    /// <returns>The root node of the generated parse tree.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public override IIntermediateCodeNode Parse(Token token)
    {
        if (token.TokenType != PascalTokenType.Begin)
        {
            throw new InvalidOperationException($"Expected {PascalTokenType.Begin} but found {token.TokenType}");
        }

        token = GetNextToken();
        IIntermediateCodeNode compundNode 
            = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.Compound);
        StatementParser statementParser = new(this);
        statementParser.ParseList(token, compundNode, PascalTokenType.End, PascalErrorCode.MissingEnd);
        return compundNode;
    }
}