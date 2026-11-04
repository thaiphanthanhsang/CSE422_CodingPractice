namespace LibraryManagementSOLID.Models;

// E-books use Quantity to represent concurrent lending licenses.
public sealed class EBook : Book
{
    public EBook(int id, string title, string author, string category, int quantity, string fileFormat = "PDF")
        : base(id, title, author, category, quantity)
    {
        FileFormat = fileFormat;
    }

    public string FileFormat { get; }
    public override string TypeName => "eBook";
}
