namespace WritingCompilersAndInterpretersLib.Message;

public class MessageHandler : IObservable<Message>
{
    private readonly IList<IObserver<Message>> _observers = new List<IObserver<Message>>();

    /// <value>The number of subscribers.</value> 
    public int SubscriberCount => _observers.Count;

    /// <summary>
    /// Add a subscriber to the list of observers.
    /// </summary>
    /// <param name="observer">The subscriber to add.</param>
    /// <returns>An object which can be used to remove the subscriber.</returns>
    public IDisposable Subscribe(IObserver<Message> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubscriber<Message>(_observers, observer);
    }

    /// <summary>
    /// Send a <see cref="Message"/> to each <see cref="_observer"/>.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to send.</param>
    public void SendMessage(Message message) => _observers.ToList().ForEach(o => o.OnNext(message));

    /// <summary>
    /// Is the given subscriber in the list of observers.
    /// </summary>
    /// <param name="observer">The subscriber to check.</param>
    /// <returns><see langword="true"/> if the subscriber is in the list of observers; otherwise 
    /// <see langword="false"/>.</returns>
    public bool IsSubscriber(IObserver<Message> observer) => _observers.Contains(observer);
}
