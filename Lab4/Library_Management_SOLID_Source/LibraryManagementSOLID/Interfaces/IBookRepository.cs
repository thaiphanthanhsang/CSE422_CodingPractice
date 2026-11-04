using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface IBookRepository
{
    void Add(Book book);
    Book? GetById(int id);
    IReadOnlyCollection<Book> GetAll();
}
