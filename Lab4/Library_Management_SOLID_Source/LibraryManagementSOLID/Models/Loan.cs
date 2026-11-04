namespace LibraryManagementSOLID.Models;

// A loan represents one borrowed copy, even for an eBook license.
public sealed class Loan
{
    public Loan(int id, int readerId, int bookId, DateTimeOffset borrowedAt)
    {
        if (id <= 0 || readerId <= 0 || bookId <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "IDs must be positive.");
        Id = id;
        ReaderId = readerId;
        BookId = bookId;
        BorrowedAt = borrowedAt;
    }

    public int Id { get; }
    public int ReaderId { get; }
    public int BookId { get; }
    public DateTimeOffset BorrowedAt { get; }
    public DateTimeOffset? ReturnedAt { get; private set; }
    public bool IsActive => ReturnedAt is null;

    public void MarkReturned(DateTimeOffset returnedAt)
    {
        if (!IsActive) throw new InvalidOperationException("Loan was already returned.");
        if (returnedAt < BorrowedAt)
            throw new ArgumentException("Return time cannot precede borrow time.");
        ReturnedAt = returnedAt;
    }
}
