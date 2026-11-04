using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Factories;

public sealed class MagazineFactory : IBookFactory
{
    public string Key => "3";
    public string DisplayName => "Magazine";
    public Book Create(int id, string title, string author, string category, int quantity)
        => new Magazine(id, title, author, category, quantity);
}
