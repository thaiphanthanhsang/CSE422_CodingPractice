using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface IBorrowingPolicy
{
    void EnsureCanBorrow(int readerId, int bookId, IReadOnlyCollection<Loan> activeLoans);
}
