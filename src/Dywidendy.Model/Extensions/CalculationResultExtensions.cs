using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Dywidendy.Model.Extensions
{
    public static class CalculationResultExtensions
    {
        public static RateDifferentialResult GetRateDifferential(this CalculationResult @this)
        {
            var refRate = @this.First().Rate;
            return
                new RateDifferentialResult(
                    @this.Result.Skip(1).Select(p => new RateDifferential(refRate, p.Rate, p.Money)));
        }
    }

    public class RateDifferentialResult : IEnumerable<RateDifferential>
    {
        private readonly IEnumerable<RateDifferential> _items;

        public RateDifferentialResult(IEnumerable<RateDifferential> items)
        {
            _items = items.ToList();
        }

        public IEnumerable<RateDifferential> ByItem
        {
            get { return _items.AsEnumerable(); }
        }

        public decimal JustMoney
        {
            get { return _items.Sum(p => p.Money); }
        }

        public IEnumerator<RateDifferential> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct RateDifferential
    {
        public decimal RefRate { get; }
        public decimal ActualRate { get; }
        public decimal CurrencyAmount { get; }

        public RateDifferential(decimal refRate, decimal actualRate, decimal currencyAmount)
        {
            RefRate = refRate;
            ActualRate = actualRate;
            CurrencyAmount = currencyAmount;
        }

        public decimal Money
        {
            get
            {
                return (ActualRate - RefRate)*CurrencyAmount;
            }
        }
    }
}
