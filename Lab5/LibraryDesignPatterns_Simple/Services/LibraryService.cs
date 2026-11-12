using LibraryDesignPatterns.Database;
using LibraryDesignPatterns.FactoryMethod;
using LibraryDesignPatterns.Observer;
using LibraryDesignPatterns.Strategy;
using Microsoft.Data.SqlClient;

namespace LibraryDesignPatterns.Services;

public class LibraryService
{
    private readonly DatabaseConnection _db = DatabaseConnection.Instance;
    private readonly LibraryNotifier _notifier = new();

    public void Subscribe(string name) => _notifier.Subscribe(new UserSubscriber(name));

    public void AddDocument(string type, string title, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        DocumentCreator creator = type.Trim().ToLowerInvariant() switch
        {
            "book" => new BookCreator(),
            "magazine" => new MagazineCreator(),
            "newspaper" => new NewspaperCreator(),
            _ => throw new ArgumentException("Choose book, magazine or newspaper.")
        };
        IDocument document = creator.Create(title);
        using var connection = _db.OpenConnection();
        using var command = new SqlCommand(@"
            INSERT INTO Documents(Title, DocumentType, Quantity)
            VALUES (@title, @type, @quantity)", connection);
        command.Parameters.AddWithValue("@title", document.Title);
        command.Parameters.AddWithValue("@type", document.Type);
        command.Parameters.AddWithValue("@quantity", quantity);
        command.ExecuteNonQuery();
        _notifier.Notify($"New {document.Type}: {document.Title}");
        Console.WriteLine("Document added.");
    }

    public void ShowDocuments()
    {
        using var connection = _db.OpenConnection();
        using var command = new SqlCommand("SELECT Id, Title, DocumentType, Quantity FROM Documents ORDER BY Id", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)} | Available: {reader.GetInt32(3)}");
    }

    public void AddUser(string name)
    {
        using var connection = _db.OpenConnection();
        using var command = new SqlCommand("INSERT INTO Users(Name) VALUES (@name)", connection);
        command.Parameters.AddWithValue("@name", name);
        command.ExecuteNonQuery();
        Console.WriteLine("User added.");
    }

    public void ShowUsers()
    {
        using var connection = _db.OpenConnection();
        using var command = new SqlCommand("SELECT Id, Name FROM Users ORDER BY Id", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetString(1)}");
    }


    public void Borrow(int documentId, int userId, int days)
    {
        if (days <= 0)
            throw new ArgumentException("Days must be positive.");

        using var connection = _db.OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            // Get user name
            using var userCmd = new SqlCommand(
                "SELECT Name FROM Users WHERE Id = @userId",
                connection, transaction);

            userCmd.Parameters.AddWithValue("@userId", userId);

            string userName = Convert.ToString(userCmd.ExecuteScalar())
                ?? throw new Exception("User not found.");

            // Update quantity
            using var update = new SqlCommand(@"
            UPDATE Documents
            SET Quantity = Quantity - 1
            OUTPUT inserted.DocumentType, inserted.Title
            WHERE Id = @id AND Quantity > 0",
                connection, transaction);

            update.Parameters.AddWithValue("@id", documentId);

            string type, title;

            using (var reader = update.ExecuteReader())
            {
                if (!reader.Read())
                    throw new Exception(
                        "Document not found or out of stock.");

                type = reader.GetString(0);
                title = reader.GetString(1);
            }

            // Calculate fee
            IFeeStrategy strategy = type switch
            {
                "book" => new BookFeeStrategy(),
                "magazine" => new MagazineFeeStrategy(),
                "newspaper" => new NewspaperFeeStrategy(),
                _ => throw new Exception("Unknown document type.")
            };

            decimal fee = new Loan(strategy).GetFee(days);

            // Insert loan
            using var insert = new SqlCommand(@"
            INSERT INTO Loans
                (DocumentId, UserId, BorrowDate,
                 Days, Fee, IsReturned)
            VALUES
                (@doc, @user, GETDATE(),
                 @days, @fee, 0)",
                connection, transaction);

            insert.Parameters.AddWithValue("@doc", documentId);
            insert.Parameters.AddWithValue("@user", userId);
            insert.Parameters.AddWithValue("@days", days);
            insert.Parameters.AddWithValue("@fee", fee);

            insert.ExecuteNonQuery();

            transaction.Commit();
            string documentName = $"{type} {title}";

            // Display result
            Console.WriteLine($"{documentName} borrowed by {userName}. Fee: {fee:N0} VND");

            // Observer notification
            _notifier.Notify($"{documentName} borrowed by {userName}");
        }
        catch
        {
            if (transaction.Connection != null)
                transaction.Rollback();

            throw;
        }
    }




    public void Return(int loanId)
    {
        using var connection = _db.OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Get user name
            using var userCmd = new SqlCommand(@"
            SELECT u.Name
            FROM Loans l
            JOIN Users u ON l.UserId = u.Id
            WHERE l.Id = @loan",
                connection, transaction);

            userCmd.Parameters.AddWithValue("@loan", loanId);

            string userName = Convert.ToString(userCmd.ExecuteScalar())
                ?? throw new Exception("User not found.");

            // 2. Update loan status
            using var updateLoan = new SqlCommand(@"
            UPDATE Loans
            SET IsReturned = 1, ReturnDate = GETDATE()
            OUTPUT inserted.DocumentId
            WHERE Id = @loan AND IsReturned = 0",
                connection, transaction);

            updateLoan.Parameters.AddWithValue("@loan", loanId);

            object? documentId = updateLoan.ExecuteScalar();

            if (documentId == null)
                throw new Exception("Loan not found or already returned.");

            // 3. Update document quantity
            using var updateStock = new SqlCommand(@"
            UPDATE Documents
            SET Quantity = Quantity + 1
            OUTPUT inserted.DocumentType, inserted.Title
            WHERE Id = @id",
                connection, transaction);

            updateStock.Parameters.AddWithValue("@id", (int)documentId);

            string type, title;

            using (var reader = updateStock.ExecuteReader())
            {
                if (!reader.Read())
                    throw new Exception("Document missing.");

                type = reader.GetString(0);
                title = reader.GetString(1);
            }

            transaction.Commit();

            // 4. Display result
            string documentName =
                $"{char.ToUpper(type[0])}{type.Substring(1)} {title}";

            Console.WriteLine(
                $"{documentName} returned by {userName}");

            // 5. Observer notification
            _notifier.Notify(
                $"{documentName} returned by {userName}");
        }
        catch
        {
            if (transaction.Connection != null)
                transaction.Rollback();

            throw;
        }
    }



    public void ShowLoans()
    {
        using var connection = _db.OpenConnection();
        using var command = new SqlCommand(@"
            SELECT l.Id, u.Name, d.Title, l.Days, l.Fee, l.IsReturned
            FROM Loans l
            JOIN Users u ON u.Id = l.UserId
            JOIN Documents d ON d.Id = l.DocumentId
            ORDER BY l.Id", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"Loan {reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)} | {reader.GetInt32(3)} days | {reader.GetDecimal(4):N0} VND | {(reader.GetBoolean(5) ? "Returned" : "Borrowed")}");
    }
}
