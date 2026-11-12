namespace LibraryDesignPatterns.FactoryMethod;
public class NewspaperCreator : DocumentCreator
{
    public override IDocument Create(string title) => new Newspaper(title);
}
