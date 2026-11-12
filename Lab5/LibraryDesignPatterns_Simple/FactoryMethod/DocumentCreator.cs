namespace LibraryDesignPatterns.FactoryMethod;
// Factory Method: subclasses decide which document to create.
public abstract class DocumentCreator
{
    public abstract IDocument Create(string title);
}
