namespace LibraryManagementSystem.Models;

public class PremiumMember : Member
{
    public DateTime MembershipExpiry { get; set; }
    public override int MaxBooksAllowed { get; }

    public PremiumMember(string memberId, string name, string email,
        DateTime membershipExpiry, int maxBooksAllowed)
        : base(memberId, name, email)
    {
        if (maxBooksAllowed <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxBooksAllowed));
        MembershipExpiry = membershipExpiry;
        MaxBooksAllowed = maxBooksAllowed;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"  Premium until: {MembershipExpiry:yyyy-MM-dd} | Limit: {MaxBooksAllowed}");
    }

    public override void BorrowBook(Book book)
    {
        if (MembershipExpiry.Date < DateTime.Today)
            throw new InvalidOperationException("Premium membership has expired.");
        base.BorrowBook(book);
    }
}
