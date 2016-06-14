using System;

namespace Dywidendy.Model
{
    public class ChangeDepositEvent : IChangeDepositEvent
    {
        public ChangeDepositEvent(decimal value, decimal rate, DateTime date)
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