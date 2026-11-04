using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Factories;

public sealed class EBookFactory : IBookFactory
{
    public string Key => "2";
    public string DisplayName => "eBook (quantity = concurrent licenses)";
    public Book Create(int id, string title, string author, string category, int quantity)
        => new EBook(id, title, author, category, quantity);
}
