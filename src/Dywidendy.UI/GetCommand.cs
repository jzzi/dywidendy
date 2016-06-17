using System;
using System.Windows.Input;
using Dywidendy.Model;
using Dywidendy.Model.Extensions;

namespace Dywidendy.UI
{
    public class GetCommand : ICommand
    {
        private readonly WplacViewModel _owner;
        private readonly CurrencyModel _model;

        public GetCommand(WplacViewModel owner, CurrencyModel model)
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
            _owner.ResultComputed = _model.Get(_owner.ValueToGet);
            _owner.RateDifferential = _owner.ResultComputed.GetRateDifferential(_owner.RateToAdd);
            _owner.CurrencyAmount = _model.CurrencyAmount();
            foreach (var item in _owner.ResultComputed)
            {
                _owner.Events.Add(new ChangeDepositEvent(-item.Money, item.Rate, DateTime.Now));
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}