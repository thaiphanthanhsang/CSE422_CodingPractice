using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Services;

public sealed class ReaderService : IReaderService
{
    private readonly IReaderRepository _readers;
    public ReaderService(IReaderRepository readers) => _readers = readers;

    public void Add(Reader reader) => _readers.Add(reader);
    public Reader? GetById(int id) => _readers.GetById(id);
    public IReadOnlyCollection<Reader> GetAll() => _readers.GetAll();
}
