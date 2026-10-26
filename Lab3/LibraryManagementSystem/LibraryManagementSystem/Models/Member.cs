using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models;

public class Member : IMemberActions, IPrintable
{
    private readonly List<Book> _borrowedBooks = new();

    public string MemberID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IReadOnlyList<Book> BorrowedBooks => _borrowedBooks;
    public virtual int MaxBooksAllowed => 3;

    public Member(string id, string name, string email)
    {
        MemberID = id;
        Name = name;
        Email = email;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {MemberID}, Name: {Name}, Email: {Email}");
    }

    public void PrintDetails() => DisplayInfo();

    public virtual void BorrowBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (_borrowedBooks.Count >= MaxBooksAllowed)
            throw new InvalidOperationException($"Borrow limit of {MaxBooksAllowed} books reached.");
        if (book.CopiesAvailable <= 0)
            throw new InvalidOperationException($"No copies of '{book.Title}' are available.");

        book.CopiesAvailable--;
        _borrowedBooks.Add(book);
    }

    public virtual void ReturnBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);
        if (!_borrowedBooks.Remove(book))
            throw new InvalidOperationException($"'{book.Title}' was not borrowed by this member.");

        book.CopiesAvailable++;
    }
}
