using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _books;

    public BookService(IBookRepository books) => _books = books;

    public void Add(Book book) => _books.Add(book);
    public Book? GetById(int id) => _books.GetById(id);
    public IReadOnlyCollection<Book> GetAll() => _books.GetAll();

    public IReadOnlyCollection<Book> SearchByTitle(string keyword) =>
        _books.GetAll().Where(b => b.Title.Contains(keyword ?? string.Empty,
            StringComparison.OrdinalIgnoreCase)).ToArray();

    public IReadOnlyCollection<Book> SearchByCategory(string keyword) =>
        _books.GetAll().Where(b => b.Category.Contains(keyword ?? string.Empty,
            StringComparison.OrdinalIgnoreCase)).ToArray();
}
