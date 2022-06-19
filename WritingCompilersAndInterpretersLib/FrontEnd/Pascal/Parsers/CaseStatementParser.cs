using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers
{
    internal class CaseStatementParser : StatementParser
    {
        public CaseStatementParser(StatementParser parent) : base(parent)
        {
        }

        public override IIntermediateCodeNode Parse(Token token) => throw new NotImplementedException();
    }
}