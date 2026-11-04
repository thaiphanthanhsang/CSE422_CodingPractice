using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface ILoanRepository
{
    void Add(Loan loan);
    IReadOnlyCollection<Loan> GetAll();
}
