using System;
using System.Linq;
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
            _model.Withdrawn(_owner.GetViewModel.Value, _owner.GetViewModel.Rate, _owner.GetViewModel.Date);
            _owner.RefreshValues();
        }

        public event EventHandler CanExecuteChanged;
    }
}