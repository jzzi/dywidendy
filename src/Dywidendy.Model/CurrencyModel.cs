using System;
using System.Collections.Generic;
using System.Linq;

namespace Dywidendy.Model
{
    public class CurrencyModel
    {
        public CurrencyModel(Func<IEnumerable<IChangeDepositEvent>> getEvents)
        {
            _queue = new Queue<Deposit>();
            _toDoQueue = new Queue<ToDoItem>();
            _events = new List<IChangeDepositEvent>();
            Withdrawns = new List<Withdrawn>();
            Restore(getEvents);
        }

        public IList<Withdrawn> Withdrawns { get; }

        public IList<IChangeDepositEvent> Events
        {
            get { return _events.ToArray(); }
        }

        private Deposit _currentDeposit;
        private decimal _moneyOfCurrentRate;


        private readonly Queue<Deposit> _queue;
        private readonly Queue<ToDoItem> _toDoQueue;
        private readonly IList<IChangeDepositEvent> _events;

        public void Deposit(decimal ammount, decimal rate, DateTime date)
        {
            if (rate <= 0) throw new RateMustBePositveException();
            _queue.Enqueue(new Deposit(ammount, rate, date));
            if (_currentDeposit == null)
            {
                SetFirstRate();
            }
            _events.Add(new MoneyChanged(ammount, rate, date));
            Withdrawn todo = null; 
            while ((todo = Withdrawns.FirstOrDefault(p=>p.ToDoMoney > 0)) != null && _currentDeposit!= null)
            {
                Withdrawn(todo);
            }
        }

        private void SetFirstRate()
        {
            ProcceedToNextRate();
        }

        public void Withdrawn(decimal value, decimal rate, DateTime date)
        {
            _events.Add(new MoneyChanged(-value, rate, date));
            var withdrawn = new Withdrawn(value, rate, date);
            Withdrawns.Add(withdrawn);
            Withdrawn(withdrawn);
        }

        private void Withdrawn(Withdrawn withdrawn)
        {
            while (true)
            {
                var toDoMoney = withdrawn.ToDoMoney;
                if (_currentDeposit == null) break;
                if (_moneyOfCurrentRate >= withdrawn.ToDoMoney)
                {
                    _moneyOfCurrentRate -= toDoMoney;
                    withdrawn.AddParentDeposit(new Deposit(toDoMoney, _currentDeposit.Rate,
                        _currentDeposit.Date));
                    break;
                }
                else
                {
                    withdrawn.AddParentDeposit(new Deposit(_moneyOfCurrentRate, _currentDeposit.Rate, _currentDeposit.Date));
                    _moneyOfCurrentRate = 0;
                    if (_queue.Any())
                    {
                        ProcceedToNextRate();
                    }
                    else
                    {
                       _currentDeposit = null;
                        break;
                    }
                }
            }
        }

        private void ProcceedToNextRate()
        {
            var current = _queue.Dequeue();
            _currentDeposit = current;
            _moneyOfCurrentRate = current.Ammount;
        }

        private void Restore(Func<IEnumerable<IChangeDepositEvent>> getEvents)
        {
            foreach (var item in getEvents())
            {
                if (item.Value > 0)
                {
                    Deposit(item.Value, item.Rate, item.Date);
                }
                else
                {
                    Withdrawn(-item.Value, item.Rate, item.Date);
                }
            }
        }

        public decimal CurrencyAmount()
        {
            return _queue.AsEnumerable().Sum(p => p.Ammount) + _moneyOfCurrentRate;
        }

        public void Save(Action<IEnumerable<IChangeDepositEvent>> saveEvents)
        {
            saveEvents(_events);
        }

    }

    public class Withdrawn
    {
        public decimal Ammount { get; }
        public decimal RefRate { get; }
        public DateTime Date { get; }
        public IList<Deposit> Deposits { get; }

        public Withdrawn(decimal ammount, decimal refRate, DateTime date)
        {
            Ammount = ammount;
            RefRate = refRate;
            Date = date;
            Deposits = new List<Deposit>();
        }

        public decimal ToDoMoney { get { return Ammount - Deposits.Sum(p => p.Ammount); } }
        public decimal? RateDifferential { get { return ToDoMoney > 0 ? null : (decimal?)(Ammount * RefRate) - Deposits.Sum(p => p.Ammount * p.Rate); } }

        public void AddParentDeposit(Deposit deposit)
        {
            Deposits.Add(deposit);
        }
    }

    public class Deposit
    {
        public Deposit(decimal ammount, decimal rate, DateTime date)
        {
            Ammount = ammount;
            Rate = rate;
            Date = date;
        }

        public decimal Ammount { get; }
        public decimal Rate { get; }
        public DateTime Date { get; }
    }
}