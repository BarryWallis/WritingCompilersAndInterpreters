namespace WritingCompilersAndInterpretersLib.Message;

public class Unsubscriber<T> : IDisposable
{
    private readonly IList<IObserver<Message>> _observers;
    private readonly IObserver<Message> _observer;

    /// <summary>
    /// Create an <see cref="Unsubscriber{T}"/> object that can be used to remove the observer from the list of observers.
    /// </summary>
    /// <param name="observers">The list of observers.</param>
    /// <param name="observer">The observer to remove.</param>
    public Unsubscriber(IList<IObserver<Message>> observers, IObserver<Message> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    /// <summary>
    /// Remove the <see cref="_observer"/> from the list of <see cref="_observers"/>.
    /// </summary>
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose() => _ = _observers.Remove(_observer);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
}