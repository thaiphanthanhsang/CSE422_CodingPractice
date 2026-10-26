namespace LibraryManagementSystem.Models;

public class LibraryCard
{
    public string CardNumber { get; }
    public Member Owner { get; set; }
    public DateTime IssueDate { get; private set; }

    public LibraryCard(string cardNumber, Member owner)
    {
        CardNumber = cardNumber;
        Owner = owner;
        IssueDate = DateTime.Today;
    }

    public void RenewCard() => IssueDate = DateTime.Today;
}
