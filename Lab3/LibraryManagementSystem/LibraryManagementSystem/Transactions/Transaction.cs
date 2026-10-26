using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Transactions;

public abstract class Transaction
{
    public string TransactionID { get; }
    public DateTime TransactionDate { get; }
    public Member Member { get; }
    protected Library Library { get; }

    protected Transaction(string transactionId, Member member, Library library)
    {
        TransactionID = transactionId;
        TransactionDate = DateTime.Now;
        Member = member;
        Library = library;
    }

    public abstract void Execute();
}
