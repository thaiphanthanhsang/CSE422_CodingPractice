namespace LibraryManagementSOLID.Models;

public sealed class Reader
{
    public Reader(int id, string name)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Reader name is required.");
        Id = id;
        Name = name.Trim();
    }

    public int Id { get; }
    public string Name { get; }
}
