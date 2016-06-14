using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dywidendy.Model
{
    public class CalculationResult : IEnumerable<MoneyWithRate>
    {
        private CalculationResult()
        {
            _result = new List<MoneyWithRate>();
        }

        public static CalculationResult Empty()
        {
            return new CalculationResult();
        }

        private CalculationResult(IEnumerable<MoneyWithRate> source)
        {
            _result = source;
        }

        public CalculationResult Add(MoneyWithRate right)
        {
            var temp = _result.ToList();
            temp.Add(right);
            return new CalculationResult(temp);
        }
        private readonly IEnumerable<MoneyWithRate> _result;

        public List<MoneyWithRate> Result => _result.ToList();
        public IEnumerator<MoneyWithRate> GetEnumerator()
        {
            return Result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}