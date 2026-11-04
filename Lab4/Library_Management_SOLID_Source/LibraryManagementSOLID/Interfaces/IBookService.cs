using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

public interface IBookService
{
    void Add(Book book);
    Book? GetById(int id);
    IReadOnlyCollection<Book> GetAll();
    IReadOnlyCollection<Book> SearchByTitle(string keyword);
    IReadOnlyCollection<Book> SearchByCategory(string keyword);
}
