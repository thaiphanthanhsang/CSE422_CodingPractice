using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Transactions;

public class ReturnTransaction : Transaction
{
    public Book BookReturned { get; }

    public ReturnTransaction(string id, Member member, Library library, Book book)
        : base(id, member, library) => BookReturned = book;

    public override void Execute() => Library.ReturnBook(Member, BookReturned);
}
