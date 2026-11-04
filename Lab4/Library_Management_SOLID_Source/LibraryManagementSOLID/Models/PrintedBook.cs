namespace LibraryManagementSOLID.Models;

public sealed class PrintedBook : Book
{
    public PrintedBook(int id, string title, string author, string category, int quantity)
        : base(id, title, author, category, quantity) { }

    public override string TypeName => "Printed Book";
}
