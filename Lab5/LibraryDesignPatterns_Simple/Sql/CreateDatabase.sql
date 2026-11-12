IF DB_ID('LibraryDB') IS NULL CREATE DATABASE LibraryDB;
GO
USE LibraryDB;
GO
IF OBJECT_ID('dbo.Documents', 'U') IS NULL
CREATE TABLE Documents (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    DocumentType VARCHAR(20) NOT NULL CHECK (DocumentType IN ('book','magazine','newspaper')),
    Quantity INT NOT NULL CHECK (Quantity >= 0)
);
GO
IF OBJECT_ID('dbo.Users', 'U') IS NULL
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO
IF OBJECT_ID('dbo.Loans', 'U') IS NULL
CREATE TABLE Loans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DocumentId INT NOT NULL REFERENCES Documents(Id),
    UserId INT NOT NULL REFERENCES Users(Id),
    BorrowDate DATETIME NOT NULL,
    ReturnDate DATETIME NULL,
    Days INT NOT NULL CHECK (Days > 0),
    Fee DECIMAL(18,2) NOT NULL,
    IsReturned BIT NOT NULL DEFAULT 0
);
GO
