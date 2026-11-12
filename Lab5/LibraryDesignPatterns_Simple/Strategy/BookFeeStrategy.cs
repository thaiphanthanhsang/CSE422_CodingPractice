namespace LibraryDesignPatterns.Strategy;
public class BookFeeStrategy : IFeeStrategy
{
    public decimal Calculate(int days) => days * 2000m;
}

