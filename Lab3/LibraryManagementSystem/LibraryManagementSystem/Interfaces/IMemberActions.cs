using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IMemberActions
{
    void BorrowBook(Book book);
    void ReturnBook(Book book);
}
