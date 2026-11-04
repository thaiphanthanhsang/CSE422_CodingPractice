using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;

namespace LibraryManagementSOLID.Data;

public static class SampleDataSeeder
{
    public static void Seed(IBookService books, IReaderService readers,
        IIdGenerator bookIds, IIdGenerator readerIds, IReadOnlyCollection<IBookFactory> factories)
    {
        var kinds = factories.ToDictionary(factory => factory.Key);
        books.Add(kinds["1"].Create(bookIds.NextId(), "Clean Code", "Robert C. Martin", "Programming", 3));
        books.Add(kinds["1"].Create(bookIds.NextId(), "Design Patterns", "GoF", "Programming", 2));
        books.Add(kinds["1"].Create(bookIds.NextId(), "The Pragmatic Programmer", "Hunt & Thomas", "Programming", 1));
        books.Add(kinds["1"].Create(bookIds.NextId(), "Database Systems", "Ramez Elmasri", "Database", 0));
        books.Add(kinds["2"].Create(bookIds.NextId(), "C# Digital Handbook", "Learning Team", "Programming", 2));
        books.Add(kinds["3"].Create(bookIds.NextId(), "Science Monthly", "Editorial Board", "Science", 2));

        readers.Add(new Reader(readerIds.NextId(), "Nguyen Van An"));
        readers.Add(new Reader(readerIds.NextId(), "Tran Thi Binh"));
        readers.Add(new Reader(readerIds.NextId(), "Le Minh Chau"));
    }
}
