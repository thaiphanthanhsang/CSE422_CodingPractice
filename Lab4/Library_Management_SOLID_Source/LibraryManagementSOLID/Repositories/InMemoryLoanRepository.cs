using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Repositories;

public sealed class InMemoryLoanRepository : ILoanRepository
{
    private readonly Dictionary<int, Loan> _loans = new();

    public void Add(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        if (!_loans.TryAdd(loan.Id, loan))
            throw new InvalidOperationException($"Loan ID {loan.Id} already exists.");
    }

    public IReadOnlyCollection<Loan> GetAll() => _loans.Values.OrderBy(l => l.Id).ToArray();
}
