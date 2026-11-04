using System.Text;
using LibraryManagementSOLID.Data;
using LibraryManagementSOLID.Factories;
using LibraryManagementSOLID.Infrastructure;
using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;
using LibraryManagementSOLID.Policies;
using LibraryManagementSOLID.Repositories;
using LibraryManagementSOLID.Services;
using LibraryManagementSOLID.Tests;

namespace LibraryManagementSOLID;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Contains("--test", StringComparer.OrdinalIgnoreCase))
        {
            Environment.ExitCode = SmokeTests.Run();
            return;
        }

        // Composition root: concrete classes are wired up only here.
        IBookRepository bookRepository = new InMemoryBookRepository();
        IReaderRepository readerRepository = new InMemoryReaderRepository();
        ILoanRepository loanRepository = new InMemoryLoanRepository();
        IIdGenerator bookIds = new SequentialIdGenerator();
        IIdGenerator readerIds = new SequentialIdGenerator();
        IIdGenerator loanIds = new SequentialIdGenerator();

        IBookService books = new BookService(bookRepository);
        IReaderService readers = new ReaderService(readerRepository);
        ILendingService lending = new LendingService(bookRepository, readerRepository,
            loanRepository, new MaxThreeBooksPolicy(), loanIds);
        IReportService report = new ReportService(readerRepository, bookRepository, loanRepository);

        IReadOnlyCollection<IBookFactory> factories = new IBookFactory[]
        {
            new PrintedBookFactory(), new EBookFactory(), new MagazineFactory()
        };

        SampleDataSeeder.Seed(books, readers, bookIds, readerIds, factories);
        ShowMenu(books, readers, lending, report, bookIds, readerIds, factories);
    }

    private static void ShowMenu(IBookService books, IReaderService readers,
        ILendingService lending, IReportService report, IIdGenerator bookIds,
        IIdGenerator readerIds, IReadOnlyCollection<IBookFactory> factories)
    {
        while (true)
        {
            Console.WriteLine("\n========== LIBRARY MANAGEMENT (SOLID) ==========");
            Console.WriteLine("1. List books                 2. Add a book");
            Console.WriteLine("3. Search by title            4. Search by category");
            Console.WriteLine("5. List readers               6. Add a reader");
            Console.WriteLine("7. Borrow a book              8. Return a book");
            Console.WriteLine("9. Borrowing report           0. Exit");
            Console.Write("Choose: ");
            var choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1": DisplayBooks(books.GetAll()); break;
                    case "2": AddBook(books, bookIds, factories); break;
                    case "3": DisplayBooks(books.SearchByTitle(ReadNonEmpty("Title keyword: "))); break;
                    case "4": DisplayBooks(books.SearchByCategory(ReadNonEmpty("Category keyword: "))); break;
                    case "5": DisplayReaders(readers); break;
                    case "6": AddReader(readers, readerIds); break;
                    case "7":
                    {
                        var readerId = ReadPositiveInt("Reader ID: ");
                        var bookId = ReadPositiveInt("Book ID: ");
                        var loan = lending.Borrow(readerId, bookId);
                        Console.WriteLine($"Borrowed successfully. Loan ID: {loan.Id}");
                        break;
                    }
                    case "8":
                        lending.Return(ReadPositiveInt("Reader ID: "), ReadPositiveInt("Book ID: "));
                        Console.WriteLine("Book returned successfully.");
                        break;
                    case "9": Console.WriteLine(report.GenerateBorrowingReport()); break;
                    case "0": Console.WriteLine("Goodbye!"); return;
                    default: Console.WriteLine("Invalid choice. Select 0-9."); break;
                }
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void AddBook(IBookService books, IIdGenerator bookIds,
        IReadOnlyCollection<IBookFactory> factories)
    {
        Console.WriteLine("Available types:");
        foreach (var factory in factories)
            Console.WriteLine($"  {factory.Key}: {factory.DisplayName}");
        var type = ReadNonEmpty("Type key: ");
        var selected = factories.FirstOrDefault(factory => factory.Key == type)
            ?? throw new InvalidOperationException("Unknown book type.");
        var title = ReadNonEmpty("Title: ");
        var author = ReadNonEmpty("Author: ");
        var category = ReadNonEmpty("Category: ");
        var quantity = ReadNonNegativeInt("Quantity: ");
        var book = selected.Create(bookIds.NextId(), title, author, category, quantity);
        books.Add(book);
        Console.WriteLine($"Book added. New ID: {book.Id}");
    }

    private static void AddReader(IReaderService readers, IIdGenerator readerIds)
    {
        var reader = new Reader(readerIds.NextId(), ReadNonEmpty("Reader name: "));
        readers.Add(reader);
        Console.WriteLine($"Reader added. New ID: {reader.Id}");
    }

    private static void DisplayBooks(IReadOnlyCollection<Book> books)
    {
        if (books.Count == 0) { Console.WriteLine("No books found."); return; }
        Console.WriteLine("ID | Type         | Title                         | Author                  | Category       | Stock");
        Console.WriteLine(new string('-', 105));
        foreach (var book in books)
            Console.WriteLine($"{book.Id,2} | {Short(book.TypeName,12),-12} | {Short(book.Title,29),-29} | " +
                $"{Short(book.Author,23),-23} | {Short(book.Category,14),-14} | {book.Quantity,5}");
    }

    private static string Short(string text, int length) =>
        text.Length <= length ? text : text[..(length - 3)] + "...";

    private static void DisplayReaders(IReaderService readers)
    {
        foreach (var reader in readers.GetAll())
            Console.WriteLine($"Reader #{reader.Id}: {reader.Name}");
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value)) return value;
            Console.WriteLine("Value must not be empty.");
        }
    }

    private static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value > 0) return value;
            Console.WriteLine("Enter a positive integer.");
        }
    }

    private static int ReadNonNegativeInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value >= 0) return value;
            Console.WriteLine("Enter a non-negative integer.");
        }
    }
}
