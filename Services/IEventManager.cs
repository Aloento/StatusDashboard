namespace StatusDashboard.Services;

public interface IEventManager<T> : IDisposable {
    event Action? OnEvent;

    void Publish();

    void Subscribe(Action handler);

    void Unsubscribe(Action handler);
}
