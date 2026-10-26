namespace LibraryManagementSystem.Services;

public class AdvancedNotificationService : NotificationService
{
    public override void SendNotification(string message) =>
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [Advanced] {message}");
}
