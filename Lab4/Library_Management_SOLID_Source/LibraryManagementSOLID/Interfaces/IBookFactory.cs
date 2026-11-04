using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Interfaces;

// Register additional factories to add book types; do not change the services.
public interface IBookFactory
{
    string Key { get; }
    string DisplayName { get; }
    Book Create(int id, string title, string author, string category, int quantity);
}
