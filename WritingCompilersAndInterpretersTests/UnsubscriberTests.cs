using WritingCompilersAndInterpretersLib.Message;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class UnsubscriberTests
{
    internal class TestObserver : IObserver<Message>
    {
        public SourceLineMessage? SourceLineMessage { get; private set; } = null;

        public void OnCompleted() => throw new NotImplementedException();
        public void OnError(Exception error) => throw new NotImplementedException();
        public void OnNext(Message value)
            => SourceLineMessage = value as SourceLineMessage ?? throw new InvalidCastException();
    }

    [TestMethod]
    public void Dispose_Unsubscribe_SubscriberRemovedFromList()
    {
        MessageHandler messageHandler = new();
        TestObserver observer = new();
        IDisposable unsubscribe = messageHandler.Subscribe(observer);

        unsubscribe.Dispose();

        Assert.AreEqual(0, messageHandler.SubscriberCount);
        Assert.IsFalse(messageHandler.IsSubscriber(observer));
    }

    [TestMethod]
    public void Dispose_UnsubscribeSubscriberNotInList_DisposeDoesNothing()
    {
        MessageHandler messageHandler = new();
        TestObserver observer = new();
        IDisposable unsubscribe = messageHandler.Subscribe(observer);

        unsubscribe.Dispose();
        unsubscribe.Dispose();

        Assert.AreEqual(0, messageHandler.SubscriberCount);
        Assert.IsFalse(messageHandler.IsSubscriber(observer));
    }
}
