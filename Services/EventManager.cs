namespace StatusDashboard.Services;

public class EventManager<T> : IEventManager<T> {
    public event Action? OnEvent;

    public void Publish() => this.OnEvent?.Invoke();

    public void Subscribe(Action handler) => this.OnEvent += handler;

    public void Unsubscribe(Action handler) => this.OnEvent -= handler;

    public void Dispose() => this.OnEvent = null;
}
