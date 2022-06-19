using System.Diagnostics;

using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.IntermediateCodeImplementation;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers
{
    /// <summary>
    /// Parse an IF statement.
    /// </summary>
    public class IfStatementParser : StatementParser
    {
        private static readonly ISet<PascalTokenType> _thenTokenTypes;

        static IfStatementParser()
        {
            _thenTokenTypes = new HashSet<PascalTokenType>(StatementStartTokenTypes);
            _ = _thenTokenTypes.Add(PascalTokenType.Then);
            _thenTokenTypes.UnionWith(StatementFollowTokenTypes);
        }

        /// <summary>
        /// Create a new IF statement parser.
        /// </summary>
        /// <param name="parent">The parent of this parser.</param>
        public IfStatementParser(StatementParser parent) : base(parent)
        {
        }

        /// <inheritdoc/>
        public override IIntermediateCodeNode Parse(Token token)
        {
            _ = GetNextToken();
            IIntermediateCodeNode ifNode
               = IntermediateCodeFactory.CreateIntermediateCodeNode(IntermediateCodeNodeType.If);
            ParseIfExpression(ifNode);
            ParseThenExpression(ifNode);
            ParseElseExpression(ifNode);

            return ifNode;
        }

        private void ParseElseExpression(IIntermediateCodeNode ifNode)
        {
            Token token;
            Debug.Assert(CurrentToken is not null);
            token = CurrentToken;

            if (token.TokenType == PascalTokenType.Else)
            {
                token = GetNextToken();
                StatementParser statementParser = new(this);
                _ = ifNode.AddChild(statementParser.Parse(token));
            }
        }

        private void ParseThenExpression(IIntermediateCodeNode ifNode)
        {
            Token token = Synchronize(_thenTokenTypes);
            if (token.TokenType == PascalTokenType.Then)
            {
                token = GetNextToken();
            }
            else
            {
                errorHandler.Flag(token, PascalErrorCode.MissingThen, this);
            }

            StatementParser statementParser = new(this);
            _ = ifNode.AddChild(statementParser.Parse(token));
        }

        private void ParseIfExpression(IIntermediateCodeNode ifNode)
        {
            Debug.Assert(CurrentToken is not null);
            Token token = CurrentToken;
            ExpressionParser expressionParser = new(this);
            _ = ifNode.AddChild(expressionParser.Parse(token));
        }
    }
}