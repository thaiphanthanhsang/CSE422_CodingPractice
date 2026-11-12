namespace LibraryDesignPatterns.Strategy;
public class MagazineFeeStrategy : IFeeStrategy
{
    public decimal Calculate(int days) => days * 1500m;
}

