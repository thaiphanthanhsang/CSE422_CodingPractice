using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface IReaderService
{
    void Add(Reader reader);
    Reader? GetById(int id);
    IReadOnlyCollection<Reader> GetAll();
}
