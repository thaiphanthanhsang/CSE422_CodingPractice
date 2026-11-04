using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Services;

public sealed class LendingService : ILendingService
{
    private readonly IBookRepository _books;
    private readonly IReaderRepository _readers;
    private readonly ILoanRepository _loans;
    private readonly IBorrowingPolicy _policy;
    private readonly IIdGenerator _loanIds;

    public LendingService(IBookRepository books, IReaderRepository readers,
        ILoanRepository loans, IBorrowingPolicy policy, IIdGenerator loanIds)
    {
        _books = books;
        _readers = readers;
        _loans = loans;
        _policy = policy;
        _loanIds = loanIds;
    }

    public Loan Borrow(int readerId, int bookId)
    {
        if (_readers.GetById(readerId) is null)
            throw new InvalidOperationException($"Reader ID {readerId} does not exist.");

        var book = _books.GetById(bookId)
            ?? throw new InvalidOperationException($"Book ID {bookId} does not exist.");

        var activeLoans = _loans.GetAll()
            .Where(loan => loan.ReaderId == readerId && loan.IsActive).ToArray();
        _policy.EnsureCanBorrow(readerId, bookId, activeLoans);

        book.BorrowOne();
        try
        {
            var loan = new Loan(_loanIds.NextId(), readerId, bookId, DateTimeOffset.UtcNow);
            _loans.Add(loan);
            return loan;
        }
        catch
        {
            book.ReturnOne(); // rollback the in-memory stock if loan creation fails
            throw;
        }
    }

    public void Return(int readerId, int bookId)
    {
        var loan = _loans.GetAll().FirstOrDefault(l =>
            l.ReaderId == readerId && l.BookId == bookId && l.IsActive)
            ?? throw new InvalidOperationException("No active loan found for this reader and book.");

        var book = _books.GetById(bookId)
            ?? throw new InvalidOperationException("Borrowed book no longer exists.");

        loan.MarkReturned(DateTimeOffset.UtcNow);
        book.ReturnOne();
    }
}
