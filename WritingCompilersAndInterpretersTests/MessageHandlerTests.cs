using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WritingCompilersAndInterpretersLib.Message;

using static WritingCompilersAndInterpretersTests.MessageHandlerTests;

namespace WritingCompilersAndInterpretersTests;

[TestClass]
public class MessageHandlerTests
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
    public void SubscriberCount_NoSubscribers_ReturnsZero()
    {
        MessageHandler messageHandler = new();

        int actual = messageHandler.SubscriberCount;

        Assert.AreEqual(actual, 0);
    }

    [TestMethod]
    public void Subscribe_NewSubscriber_AddsToSubscriberList()
    {
        MessageHandler messageHandler = new();
        IObserver<Message> testObserver = new TestObserver() as IObserver<Message> ?? throw new InvalidCastException();

        _ = messageHandler.Subscribe(testObserver);

        Assert.AreEqual(1, messageHandler.SubscriberCount);
        Assert.IsTrue(messageHandler.IsSubscriber(testObserver));
    }

    [TestMethod]
    public void Subscribe_AddRedundantSubscriber_NoChangeToList()
    {
        MessageHandler messageHandler = new();
        IObserver<Message> testObserver = new TestObserver() as IObserver<Message> ?? throw new InvalidCastException();
        _ = messageHandler.Subscribe(testObserver);

        _ = messageHandler.Subscribe(testObserver);

        Assert.AreEqual(1, messageHandler.SubscriberCount);
        Assert.IsTrue(messageHandler.IsSubscriber(testObserver));
    }

    [TestMethod]
    public void SendMessage_SendMessageToSubscriber_MessageRecievedBySubscriber()
    {
        MessageHandler messageHandler = new();
        Message expected = new SourceLineMessage(1, "test");
        TestObserver testObserver = new();
        _ = messageHandler.Subscribe(testObserver);

        messageHandler.SendMessage(expected);

        Assert.AreEqual(expected, testObserver.SourceLineMessage);
    }

    [TestMethod]
    public void SendMessage_SendMessageToMultipleSubscribers_MessageRecievedByEachSubscriber()
    {
        MessageHandler messageHandler = new();
        Message expected = new SourceLineMessage(1, "test1");
        TestObserver testObserver1 = new();
        TestObserver testObserver2 = new();
        _ = messageHandler.Subscribe(testObserver1);
        _ = messageHandler.Subscribe(testObserver2);

        messageHandler.SendMessage(expected);

        Assert.AreEqual(expected, testObserver1.SourceLineMessage);
        Assert.AreEqual(expected, testObserver2.SourceLineMessage);
    }
}
