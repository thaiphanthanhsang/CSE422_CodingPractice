namespace LibraryDesignPatterns.FactoryMethod;
public class MagazineCreator : DocumentCreator
{
    public override IDocument Create(string title) => new Magazine(title);
}
