namespace LibraryManagementSOLID.Models;

// Shared contract for every lendable book type (printed, digital, magazine, etc.).
public abstract class Book
{
    protected Book(int id, string title, string author, string category, int quantity)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required.");
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required.");
        if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));

        Id = id;
        Title = title.Trim();
        Author = author.Trim();
        Category = category.Trim();
        Quantity = quantity;
    }

    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string Category { get; }
    public int Quantity { get; private set; }
    public abstract string TypeName { get; }

    public void BorrowOne()
    {
        if (Quantity == 0) throw new InvalidOperationException("Book is out of stock.");
        Quantity--;
    }

    public void ReturnOne() => Quantity++;
}
