namespace LibraryDesignPatterns.FactoryMethod;
public class Magazine : IDocument
{
    public string Title { get; }
    public string Type => "magazine";
    public Magazine(string title) => Title = title;
}
