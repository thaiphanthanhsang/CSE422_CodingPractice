namespace LibraryDesignPatterns.FactoryMethod;
public class BookCreator : DocumentCreator
{
    public override IDocument Create(string title) => new Book(title);
}
