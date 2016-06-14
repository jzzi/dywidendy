using System;
using System.Windows.Input;
using Dywidendy.Model;

namespace Dywidendy.UI
{
    public class AddCommand : ICommand
    {
        private readonly WplacViewModel _owner;
        private readonly CurrencyModel _model;

        public AddCommand(WplacViewModel owner, CurrencyModel model)
        {
            _owner = owner;
            _model = model;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _model.Add(new MoneyWithRate(_owner.ValueToAdd, _owner.RateToAdd));
            _owner.Events.Add(new ChangeDepositEvent(_owner.ValueToAdd, _owner.RateToAdd, DateTime.Now));
            _owner.CurrencyAmount = _model.CurrencyAmount();
        }

        public event EventHandler CanExecuteChanged;
    }
}