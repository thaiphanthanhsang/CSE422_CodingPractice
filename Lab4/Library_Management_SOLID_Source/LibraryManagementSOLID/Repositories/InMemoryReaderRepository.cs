using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Repositories;

public sealed class InMemoryReaderRepository : IReaderRepository
{
    private readonly Dictionary<int, Reader> _readers = new();

    public void Add(Reader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        if (!_readers.TryAdd(reader.Id, reader))
            throw new InvalidOperationException($"Reader ID {reader.Id} already exists.");
    }

    public Reader? GetById(int id) => _readers.GetValueOrDefault(id);
    public IReadOnlyCollection<Reader> GetAll() => _readers.Values.OrderBy(r => r.Id).ToArray();
}
