using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Transactions;

public class BorrowTransaction : Transaction
{
    public Book BookBorrowed { get; }

    public BorrowTransaction(string id, Member member, Library library, Book book)
        : base(id, member, library) => BookBorrowed = book;

    public override void Execute() => Library.BorrowBook(Member, BookBorrowed);
}
