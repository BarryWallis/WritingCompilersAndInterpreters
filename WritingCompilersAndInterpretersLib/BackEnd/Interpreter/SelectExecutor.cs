using WritingCompilersAndInterpretersLib.Intermediate;

namespace WritingCompilersAndInterpretersLib.BackEnd.Interpreter
{
    public class SelectExecutor : StatementExecutor
    {
        public SelectExecutor(Executor parent) : base(parent)
        {
        }

        public override object? Execute(IIntermediateCodeNode node) => throw new NotImplementedException();
    }
}