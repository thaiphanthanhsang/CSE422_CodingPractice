namespace LibraryDesignPatterns.Strategy;
public class NewspaperFeeStrategy : IFeeStrategy
{
    public decimal Calculate(int days) => days * 1000m;
}

