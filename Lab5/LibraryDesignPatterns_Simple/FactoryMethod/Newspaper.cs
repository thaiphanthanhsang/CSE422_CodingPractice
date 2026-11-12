namespace LibraryDesignPatterns.FactoryMethod;
public class Newspaper : IDocument
{
    public string Title { get; }
    public string Type => "newspaper";
    public Newspaper(string title) => Title = title;
}
