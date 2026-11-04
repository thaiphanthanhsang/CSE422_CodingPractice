using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Repositories;

public sealed class InMemoryBookRepository : IBookRepository
{
    private readonly Dictionary<int, Book> _books = new();

    public void Add(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (!_books.TryAdd(book.Id, book))
            throw new InvalidOperationException($"Book ID {book.Id} already exists.");
    }

    public Book? GetById(int id) => _books.GetValueOrDefault(id);
    public IReadOnlyCollection<Book> GetAll() => _books.Values.OrderBy(b => b.Id).ToArray();
}
