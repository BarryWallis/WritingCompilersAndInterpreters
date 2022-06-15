using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.FrontEnd.Pascal.Parsers;

public class ForStatementParser : StatementParser
{
    public ForStatementParser(StatementParser parent) : base(parent)
    {
    }

    public override IIntermediateCodeNode Parse(Token token) => throw new NotImplementedException();
}