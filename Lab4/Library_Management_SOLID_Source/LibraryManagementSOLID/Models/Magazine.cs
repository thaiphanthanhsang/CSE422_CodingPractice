namespace LibraryManagementSOLID.Models;

public sealed class Magazine : Book
{
    public Magazine(int id, string title, string author, string category, int quantity, int issueNumber = 1)
        : base(id, title, author, category, quantity)
    {
        if (issueNumber <= 0) throw new ArgumentOutOfRangeException(nameof(issueNumber));
        IssueNumber = issueNumber;
    }

    public int IssueNumber { get; }
    public override string TypeName => "Magazine";
}
