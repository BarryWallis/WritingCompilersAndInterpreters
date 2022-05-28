using WritingCompilersAndInterpretersLib.FrontEnd;
using WritingCompilersAndInterpretersLib.FrontEnd.Pascal;
using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class PascalParserTopDownTests
{
    internal class TestObserver : IObserver<Message>
    {
        public ParserSummaryMessage? ParserSummaryMessage { get; private set; } = null;

        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(Message value)
            => ParserSummaryMessage = value as ParserSummaryMessage ?? throw new InvalidCastException();
    }

    [TestMethod]
    public void Parse_EmptySource_SendsParserSummaryMessage()
    {
        Source source = new(new StringReader(""));
        Scanner scanner = new PascalScanner(source);
        PascalParserTopDown pascalParserTopDown = new(scanner);
        TestObserver testObserver = new();
        _ = pascalParserTopDown.Subscribe(testObserver);
        ParserSummaryMessage expected = new(0, 0, 0f);

        pascalParserTopDown.Parse();

        Assert.IsNotNull(testObserver.ParserSummaryMessage);
        Assert.AreEqual(expected.NumberOfLines, testObserver.ParserSummaryMessage.NumberOfLines);
        Assert.AreEqual(expected.ErrorCount, testObserver.ParserSummaryMessage.ErrorCount);
        Assert.AreEqual(expected.ElapsedTIme, testObserver.ParserSummaryMessage.ElapsedTIme, delta: 0.1f);
    }
}
