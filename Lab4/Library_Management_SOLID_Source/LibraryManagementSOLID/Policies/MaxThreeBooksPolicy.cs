using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Policies;

public sealed class MaxThreeBooksPolicy : IBorrowingPolicy
{
    public const int MaxActiveLoans = 3;

    public void EnsureCanBorrow(int readerId, int bookId, IReadOnlyCollection<Loan> activeLoans)
    {
        if (activeLoans.Count >= MaxActiveLoans)
            throw new InvalidOperationException("A reader may borrow at most 3 books at a time.");

        if (activeLoans.Any(loan => loan.BookId == bookId))
            throw new InvalidOperationException("This reader already has an active loan for this book.");
    }
}
