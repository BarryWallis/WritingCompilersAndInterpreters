using WritingCompilersAndInterpretersLib.BackEnd.Compiler;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class CodeGeneratorTests
{
    internal class TestObserver : IObserver<Message>
    {
        public CompilerSummaryMessage? CompilerSummaryMessage { get; private set; } = null;

        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(Message value)
            => CompilerSummaryMessage = value as CompilerSummaryMessage ?? throw new InvalidCastException();
    }

    [TestMethod]
    public void Process_AnyParameters_SendCompilerSummaryMessage()
    {
        CodeGenerator codeGenerator = new();
        TestObserver testObserver = new();
        _ = codeGenerator.Subscribe(testObserver);
        CompilerSummaryMessage expected = new(0, 0f);
        codeGenerator.Process(new IntermediateCode(), new SymbolTableStack());

        Assert.AreEqual(expected, testObserver.CompilerSummaryMessage);
    }
}
