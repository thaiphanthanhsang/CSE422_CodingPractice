using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

public class NotificationService
{
    public virtual void SendNotification(string message) =>
        Console.WriteLine($"[Notice] {message}");

    public void SendNotification(string message, string recipient) =>
        Console.WriteLine($"[To {recipient}] {message}");

    public void SendNotification(string message, List<string> recipients)
    {
        foreach (string recipient in recipients)
            SendNotification(message, recipient);
    }

    public void Subscribe(Library library) => library.OnBookBorrowed += HandleBookBorrowed;

    private void HandleBookBorrowed(Book book, Member member) =>
        SendNotification($"You borrowed '{book.Title}'.", member.Email);
}
