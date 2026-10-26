namespace LibraryManagementSystem.Models;

public class Library
{
    public string LibraryName { get; set; }
    public List<Book> Books { get; }
    public List<Member> Members { get; }
    public event Action<Book, Member>? OnBookBorrowed;

    public Library() : this("Default Library", new List<Book>()) { }

    public Library(string libraryName, List<Book> books)
    {
        LibraryName = libraryName;
        Books = new List<Book>(books);
        Members = new List<Member>();
    }

    // Shallow copy: creates separate lists but shares Book and Member objects.
    public Library(Library other)
    {
        ArgumentNullException.ThrowIfNull(other);
        LibraryName = other.LibraryName;
        Books = new List<Book>(other.Books);
        Members = new List<Member>(other.Members);
    }

    public void DisplayLibraryInfo() =>
        Console.WriteLine($"Library: {LibraryName} | Titles: {Books.Count} | Members: {Members.Count} | Available copies: {Books.Sum(b => b.CopiesAvailable)}");

    public void BorrowBook(Member member, Book book)
    {
        ValidateRegistration(member, book);
        member.BorrowBook(book);
        OnBookBorrowed?.Invoke(book, member);
    }

    public void ReturnBook(Member member, Book book)
    {
        ValidateRegistration(member, book);
        member.ReturnBook(book);
    }

    private void ValidateRegistration(Member member, Book book)
    {
        if (!Members.Contains(member))
            throw new InvalidOperationException("Member is not registered in this library.");
        if (!Books.Contains(book))
            throw new InvalidOperationException("Book is not in this library.");
    }
}
