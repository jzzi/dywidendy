using System;

namespace Dywidendy.Model
{
    public class MoneyChanged : IChangeDepositEvent
    {
        public MoneyChanged(decimal value, decimal rate, DateTime date)
        {
            Value = value;
            Rate = rate;
            Date = date;
        }

        public decimal Value { get; }
        public decimal Rate { get; }
        public DateTime Date { get; }

        
    }
}