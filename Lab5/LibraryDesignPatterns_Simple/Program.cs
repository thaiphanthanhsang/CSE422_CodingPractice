using LibraryDesignPatterns.Services;

var library = new LibraryService();
while (true)
{
    Console.WriteLine("\n=== LIBRARY MANAGEMENT ===");
    Console.WriteLine("1. Add document");
    Console.WriteLine("2. View documents");
    Console.WriteLine("3. Add user");
    Console.WriteLine("4. View users");
    Console.WriteLine("5. Borrow document");
    Console.WriteLine("6. Return document");
    Console.WriteLine("7. View loans");
    Console.WriteLine("8. Subscribe notification");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");
    string? choice = Console.ReadLine();
    if (choice == "0") break;
    try
    {
        switch (choice)
        {
            case "1":
                library.AddDocument(Ask("Type (book/magazine/newspaper): "),
                    Ask("Title: "), AskInt("Quantity: "));
                break;
            case "2": library.ShowDocuments(); break;
            case "3": library.AddUser(Ask("Name: ")); break;
            case "4": library.ShowUsers(); break;
            case "5": library.Borrow(AskInt("Document ID: "), AskInt("User ID: "), AskInt("Days: ")); break;
            case "6": library.Return(AskInt("Loan ID: ")); break;
            case "7": library.ShowLoans(); break;
            case "8": library.Subscribe(Ask("Your name: ")); Console.WriteLine("Subscribed for this session."); break;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }
    catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
}

static string Ask(string label)
{
    Console.Write(label);
    return Console.ReadLine()?.Trim() ?? "";
}
static int AskInt(string label)
{
    if (!int.TryParse(Ask(label), out int value))
        throw new ArgumentException("Please enter a number.");
    return value;
}
