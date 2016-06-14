namespace Dywidendy.Model
{
    public class MoneyWithRate
    {
        public MoneyWithRate(decimal money, decimal rate)
        {
            Rate = rate;
            Money = money;
        }

        public decimal Money { get; }
        public decimal Rate { get; }
    }
}