namespace LibraryManagementSystem.Models;

public class BookClass
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }

    public BookClass(string isbn, string title, string author)
    {
        ISBN = isbn;
        Title = title;
        Author = author;
    }
}
