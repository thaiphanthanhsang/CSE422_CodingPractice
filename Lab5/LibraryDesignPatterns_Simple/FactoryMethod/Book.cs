namespace LibraryDesignPatterns.FactoryMethod;
public class Book : IDocument
{
    public string Title { get; }
    public string Type => "book";
    public Book(string title) => Title = title;
}
