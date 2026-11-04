using LibraryManagementSOLID.Infrastructure;
using LibraryManagementSOLID.Interfaces;
using LibraryManagementSOLID.Models;
using LibraryManagementSOLID.Policies;
using LibraryManagementSOLID.Repositories;
using LibraryManagementSOLID.Services;

namespace LibraryManagementSOLID.Tests;

// Self-contained test runner: dotnet run -- --test (no NuGet packages required).
public static class SmokeTests
{
    private static int _passed;
    private static int _failed;

    public static int Run()
    {
        _passed = 0;
        _failed = 0;

        Check("Case-insensitive title/category search", () =>
        {
            var books = new BookService(new InMemoryBookRepository());
            books.Add(new PrintedBook(1, "Clean Code", "Martin", "Programming", 1));
            Ensure(books.SearchByTitle("CLEAN").Count == 1, "Title search failed");
            Ensure(books.SearchByCategory("program").Count == 1, "Category search failed");
        });

        Check("Book and reader IDs are unique", () =>
        {
            var books = new InMemoryBookRepository();
            books.Add(new PrintedBook(1, "A", "B", "C", 1));
            ExpectFailure(() => books.Add(new PrintedBook(1, "D", "E", "F", 1)));
            var readers = new InMemoryReaderRepository();
            readers.Add(new Reader(1, "An"));
            ExpectFailure(() => readers.Add(new Reader(1, "Binh")));
        });

        Check("Borrow decreases stock, return restores it", () =>
        {
            var ctx = Setup();
            ctx.Lending.Borrow(1, 1);
            Ensure(ctx.Books.GetById(1)!.Quantity == 0, "Stock did not decrease");
            ctx.Lending.Return(1, 1);
            Ensure(ctx.Books.GetById(1)!.Quantity == 1, "Stock did not restore");
        });

        Check("Cannot borrow an out-of-stock item", () =>
        {
            var ctx = Setup();
            ctx.Lending.Borrow(1, 1);
            ExpectFailure(() => ctx.Lending.Borrow(2, 1));
            Ensure(ctx.Books.GetById(1)!.Quantity == 0, "Stock went negative");
        });

        Check("Maximum of three active loans", () =>
        {
            var ctx = Setup();
            ctx.Lending.Borrow(1, 1);
            ctx.Lending.Borrow(1, 2);
            ctx.Lending.Borrow(1, 3);
            ExpectFailure(() => ctx.Lending.Borrow(1, 4));
        });

        Check("Return releases a slot for another loan", () =>
        {
            var ctx = Setup();
            ctx.Lending.Borrow(1, 1);
            ctx.Lending.Borrow(1, 2);
            ctx.Lending.Borrow(1, 3);
            ctx.Lending.Return(1, 2);
            ctx.Lending.Borrow(1, 4);
        });

        Check("Reject unknown IDs, duplicate loans, invalid returns", () =>
        {
            var ctx = Setup();
            ExpectFailure(() => ctx.Lending.Borrow(999, 1));
            ExpectFailure(() => ctx.Lending.Borrow(1, 999));
            ExpectFailure(() => ctx.Lending.Return(1, 1));
            ctx.Lending.Borrow(1, 1);
            ExpectFailure(() => ctx.Lending.Borrow(1, 1));
            ctx.Lending.Return(1, 1);
            ExpectFailure(() => ctx.Lending.Return(1, 1));
        });

        Check("Report lists readers, loan status and book titles", () =>
        {
            var ctx = Setup();
            ctx.Lending.Borrow(1, 1);
            ctx.Lending.Borrow(1, 2);
            ctx.Lending.Return(1, 2);
            var output = ctx.Report.GenerateBorrowingReport();
            Ensure(output.Contains("Alice") && output.Contains("Book 1"), "Missing reader/book");
            Ensure(output.Contains("Borrowing") && output.Contains("Returned"), "Missing statuses");
            Ensure(output.Contains("Bob"), "Reader without loans missing");
        });

        Check("Lending works with a new EBook subtype", () =>
        {
            var ctx = Setup();
            ctx.Books.Add(new EBook(5, "eBook Example", "Author", "Digital", 2));
            ctx.Lending.Borrow(1, 5);
            Ensure(ctx.Books.GetById(5)!.Quantity == 1, "eBook lending failed");
        });

        Console.WriteLine($"\nTests: {_passed} passed, {_failed} failed.");
        return _failed == 0 ? 0 : 1;
    }

    private static TestContext Setup()
    {
        IBookRepository books = new InMemoryBookRepository();
        IReaderRepository readers = new InMemoryReaderRepository();
        ILoanRepository loans = new InMemoryLoanRepository();
        for (var id = 1; id <= 4; id++)
            books.Add(new PrintedBook(id, $"Book {id}", "Author", "Test", 1));
        readers.Add(new Reader(1, "Alice"));
        readers.Add(new Reader(2, "Bob"));

        return new TestContext(books,
            new LendingService(books, readers, loans, new MaxThreeBooksPolicy(),
                new SequentialIdGenerator()),
            new ReportService(readers, books, loans));
    }

    private sealed record TestContext(IBookRepository Books, ILendingService Lending,
        IReportService Report);

    private static void Ensure(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static void ExpectFailure(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new Exception("Expected InvalidOperationException.");
    }

    private static void Check(string name, Action test)
    {
        try { test(); Console.WriteLine($"PASS: {name}"); _passed++; }
        catch (Exception ex) { Console.WriteLine($"FAIL: {name}: {ex.Message}"); _failed++; }
    }
}
