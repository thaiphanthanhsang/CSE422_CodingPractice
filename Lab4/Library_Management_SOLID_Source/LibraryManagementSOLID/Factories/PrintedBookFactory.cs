using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Factories;

public sealed class PrintedBookFactory : IBookFactory
{
    public string Key => "1";
    public string DisplayName => "Printed Book";
    public Book Create(int id, string title, string author, string category, int quantity)
        => new PrintedBook(id, title, author, category, quantity);
}
