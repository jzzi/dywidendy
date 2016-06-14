using System;
using System.Collections.Generic;
using System.Linq;

namespace Dywidendy.Model
{
    public class CurrencyModel
    {
        public CurrencyModel(Func<IEnumerable<IChangeDepositEvent>> getEvents)
        {
            _queue = new Queue<MoneyWithRate>();
            Restore(getEvents);
        }

        private decimal _currentRate;
        private decimal _moneyOfCurrentRate;

        private readonly Queue<MoneyWithRate> _queue;

        public void Add(MoneyWithRate value)
        {
            if (value.Rate <= 0) throw new RateMustBePositveException();
            _queue.Enqueue(value);
            if (_currentRate == 0)
            {
                SetFirstRate();
            }
        }

        private void SetFirstRate()
        {
            ProcceedToNextRate();
        }

        public CalculationResult Get(decimal value)
        {
            return Get(value, CalculationResult.Empty());
        }

        private CalculationResult Get(decimal value, CalculationResult current)
        {
            while (true)
            {
                if (_moneyOfCurrentRate >= value)
                {
                    _moneyOfCurrentRate -= value;
                    return current.Add(new MoneyWithRate(value, _currentRate));
                }
                else
                {
                    var moneyToDo = value - _moneyOfCurrentRate;
                    var tempResult = current.Add(new MoneyWithRate(_moneyOfCurrentRate, _currentRate));
                    _moneyOfCurrentRate = 0;
                    ProcceedToNextRate();
                    value = moneyToDo;
                    current = tempResult;
                }
            }
        }

        private void ProcceedToNextRate()
        {
            var current = _queue.Dequeue();
            _currentRate = current.Rate;
            _moneyOfCurrentRate = current.Money;
        }

        private void Restore(Func<IEnumerable<IChangeDepositEvent>> getEvents)
        {
            foreach (var item in getEvents())
            {
                if (item.Value > 0)
                {
                    Add(new MoneyWithRate(item.Value, item.Rate));
                }
                else
                {
                    Get(-item.Value);
                }
            }
        }

        public decimal CurrencyAmount()
        {
            return _queue.AsEnumerable().Sum(p => p.Money) + _moneyOfCurrentRate;
        }


    }
}