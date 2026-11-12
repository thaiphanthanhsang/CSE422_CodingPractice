namespace LibraryDesignPatterns.Observer;
public class UserSubscriber : ISubscriber
{
    public string Name { get; }
    public UserSubscriber(string name) => Name = name;
    public void Update(string message) => Console.WriteLine($"[Notification to {Name}] {message}");
}
