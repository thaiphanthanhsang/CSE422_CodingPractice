# Library Management – Simple C# + SQL Server Lab

This simplified lab has one class/interface per `.cs` file and uses four patterns.

## Setup
1. Visual Studio: Open `LibraryDesignPatterns.csproj` (Console App, .NET 8).
2. In SQL Server Object Explorer, connect to your SQL Server and execute **all** of `Sql/CreateDatabase.sql`.
3. Edit the SQL Server name in `Database/DatabaseConnection.cs` (default `.\SQLEXPRESS`). For LocalDB use `(localdb)\MSSQLLocalDB`.
4. Restore NuGet packages (`Microsoft.Data.SqlClient`) and run with Ctrl+F5.
5. Menu: add a user, add a document, view IDs, borrow, view loans, return.

## Four patterns
- Singleton: `DatabaseConnection.Instance` (one database manager; connections opened per operation).
- Factory Method: `DocumentCreator.Create()` implemented by three creator classes.
- Observer: users subscribe to `LibraryNotifier` and receive Console notifications.
- Strategy: different fee classes implement `IFeeStrategy` (Book 2000, Magazine 1500, Newspaper 1000 VND/day).

## Scope
- Notifications exist only during the current Console run, and are sent to all subscribed users; there is no email or persisted interest filtering.
- `Quantity` means currently available copies.
- This lab targets simplicity and learning, not production-grade application architecture.
- Execute the included SQL script on a dedicated lab database. It does not delete existing tables.
