using WritingCompilersAndInterpretersLib.BackEnd.Interpreter;
using WritingCompilersAndInterpretersLib.Intermediate;
using WritingCompilersAndInterpretersLib.Intermediate.SymbolTableImplementation;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class ExecutorTests
{
    internal class TestObserver : IObserver<Message>
    {
        public InterpreterSummaryMessage? InterpreterSummaryMessage { get; private set; } = null;

        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(Message value)
            => InterpreterSummaryMessage = value as InterpreterSummaryMessage ?? throw new InvalidCastException();
    }

    [TestMethod]
    public void Executor_AnyParameters_SendInterpreterSummaryMessage()
    {
        Executor executor = new();
        TestObserver testObserver = new();
        _ = executor.Subscribe(testObserver);
        InterpreterSummaryMessage expected = new(0, 0, 0f);
        executor.Process(new IntermediateCode(), new SymbolTableStack());

        Assert.AreEqual(expected, testObserver.InterpreterSummaryMessage);
    }
}

internal class IntermediateCode : IIntermediateCode
{
}
