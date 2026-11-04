using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface ILendingService
{
    Loan Borrow(int readerId, int bookId);
    void Return(int readerId, int bookId);
}
