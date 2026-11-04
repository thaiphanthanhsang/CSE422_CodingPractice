using System.Text;
using LibraryManagementSOLID.Interfaces;

namespace LibraryManagementSOLID.Services;

public sealed class ReportService : IReportService
{
    private readonly IReaderRepository _readers;
    private readonly IBookRepository _books;
    private readonly ILoanRepository _loans;

    public ReportService(IReaderRepository readers, IBookRepository books, ILoanRepository loans)
    {
        _readers = readers;
        _books = books;
        _loans = loans;
    }

    public string GenerateBorrowingReport()
    {
        var lines = new StringBuilder();
        lines.AppendLine("========== BORROWING REPORT ==========");
        lines.AppendLine("Lists both active loans and returned-book history.");

        foreach (var reader in _readers.GetAll())
        {
            var history = _loans.GetAll()
                .Where(loan => loan.ReaderId == reader.Id)
                .OrderBy(loan => loan.Id).ToArray();
            var activeCount = history.Count(loan => loan.IsActive);
            lines.AppendLine($"Reader #{reader.Id}: {reader.Name} | Active: {activeCount}");

            if (history.Length == 0)
            {
                lines.AppendLine("  (No loans)");
                continue;
            }

            foreach (var loan in history)
            {
                var book = _books.GetById(loan.BookId);
                var title = book?.Title ?? "[Deleted book]";
                var status = loan.IsActive ? "Borrowing" : "Returned";
                lines.AppendLine($"  Loan #{loan.Id} | Book #{loan.BookId}: {title} | {status} | " +
                    $"Borrowed: {loan.BorrowedAt:yyyy-MM-dd HH:mm} UTC");
            }
        }
        lines.AppendLine("======================================");
        return lines.ToString();
    }
}
