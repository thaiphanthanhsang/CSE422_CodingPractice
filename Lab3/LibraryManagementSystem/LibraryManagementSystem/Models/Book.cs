using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models;

public class Book : IPrintable
{
    private int _year;
    private int _copiesAvailable;

    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }

    public int Year
    {
        get => _year;
        set => _year = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(Year), "Year must be nonnegative.");
    }

    public int CopiesAvailable
    {
        get => _copiesAvailable;
        set => _copiesAvailable = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(CopiesAvailable), "Copies cannot be negative.");
    }

    public Book(string isbn, string title, string author, int year, int copiesAvailable)
    {
        ISBN = isbn;
        Title = title;
        Author = author;
        Year = year;
        CopiesAvailable = copiesAvailable;
    }

    public void DisplayInfo() =>
        Console.WriteLine($"ISBN: {ISBN} | Title: {Title} | Author: {Author} | Year: {Year} | Available: {CopiesAvailable}");

    public void PrintDetails() => DisplayInfo();
}
