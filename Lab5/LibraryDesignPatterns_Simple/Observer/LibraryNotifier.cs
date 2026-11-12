namespace LibraryDesignPatterns.Observer;
public class LibraryNotifier
{
    private readonly List<ISubscriber> _users = new();
    public void Subscribe(ISubscriber user) => _users.Add(user);
    public void Notify(string message)
    {
        foreach (var user in _users) user.Update(message);
    }
}
