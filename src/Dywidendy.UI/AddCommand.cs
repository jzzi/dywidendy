using System;
using System.Linq;
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
            _model.Deposit(_owner.AddViewModel.Value, _owner.AddViewModel.Rate, _owner.AddViewModel.Date);
            _owner.RefreshValues();
        }

        public event EventHandler CanExecuteChanged;
    }
}