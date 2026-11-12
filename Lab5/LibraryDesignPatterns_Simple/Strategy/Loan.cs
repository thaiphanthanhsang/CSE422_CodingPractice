namespace LibraryDesignPatterns.Strategy;
public class Loan
{
    private readonly IFeeStrategy _strategy;
    public Loan(IFeeStrategy strategy) => _strategy = strategy;
    public decimal GetFee(int days) => _strategy.Calculate(days);
}

