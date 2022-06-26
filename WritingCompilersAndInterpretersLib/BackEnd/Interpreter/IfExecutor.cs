using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter
{
    public class IfExecutor : StatementExecutor
    {
        public IfExecutor(Executor parent) : base(parent)
        {
        }

        public override object? Execute(IIntermediateCodeNode node) => throw new NotImplementedException();
    }
}