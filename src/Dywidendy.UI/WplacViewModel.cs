using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dywidendy.Model;
using Dywidendy.UI.Annotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Dywidendy.Model.Extensions;

namespace Dywidendy.UI
{
    public class WplacViewModel : INotifyPropertyChanged
    {
        private decimal _valueToAdd;
        private decimal _rateToAdd;
        private decimal _valueToGet;
        private CalculationResult _resultComputed;
        private RateDifferentialResult _rateDifferential;
        private decimal _currencyAmount;
        private ObservableCollection<IChangeDepositEvent> _events;
        private ICommand _addCommand;
        private ICommand _getCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private DateTime _dateToAdd;
        private ICommand _getRateToAddCommand;

        public DateTime DateToAdd
        {
            get { return _dateToAdd; }
            set
            {
                if (value.Equals(_dateToAdd)) return;
                _dateToAdd = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IChangeDepositEvent> Events
        {
            get { return _events; }
            private set
            {
                if (Equals(value, _events)) return;
                _events = value;                
                OnPropertyChanged();
            }
        }
               
        public WplacViewModel()
        {
            Reload(new List<IChangeDepositEvent>());
            RateToAdd = 1;
            DateToAdd = DateTime.Today;
        }

        internal void Reload(IEnumerable<IChangeDepositEvent> events)
        {
            Events = new ObservableCollection<IChangeDepositEvent>(events);
            var currencyModel = new CurrencyModel(() => Events);
            AddCommand = new AddCommand(this, currencyModel);
            GetCommand = new GetCommand(this, currencyModel);
            OpenFileCommand = new OpenFileCommand(this, currencyModel);
            SaveFileCommand = new SaveFileCommand(this);
            CurrencyAmount = currencyModel.CurrencyAmount();
        }

        public ICommand GetRateToAddCommand
        {
            get { return _getRateToAddCommand; }
            set
            {
                if (Equals(value, _getRateToAddCommand)) return;
                _getRateToAddCommand = value;
                OnPropertyChanged();
            }
        }


        public decimal ValueToAdd
        {
            get { return _valueToAdd; }
            set
            {
                if (value.Equals(_valueToAdd)) return;
                _valueToAdd = value;
                OnPropertyChanged();
            }
        }

        public decimal RateToAdd
        {
            get { return _rateToAdd; }
            set
            {
                if (value.Equals(_rateToAdd)) return;
                _rateToAdd = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand
        {
            get { return _addCommand; }
            private set
            {
                if (Equals(value, _addCommand)) return;
                _addCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetCommand
        {
            get { return _getCommand; }
            private set
            {
                if (Equals(value, _getCommand)) return;
                _getCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenFileCommand
        {
            get { return _openFileCommand; }
            private set
            {
                if (Equals(value, _openFileCommand)) return;
                _openFileCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveFileCommand
        {
            get { return _saveFileCommand; }
            private set
            {
                if (Equals(value, _openFileCommand)) return;
                _saveFileCommand = value;
                OnPropertyChanged();
            }
        }

        public decimal ValueToGet
        {
            get { return _valueToGet; }
            set
            {
                if (value.Equals(_valueToGet)) return;
                _valueToGet = value;
                OnPropertyChanged();
            }
        }

        public CalculationResult ResultComputed
        {
            get { return _resultComputed; }
            set
            {
                if (Equals(value, _resultComputed)) return;
                _resultComputed = value;
                OnPropertyChanged();
            }
        }

        public RateDifferentialResult RateDifferential
        {
            get { return _rateDifferential; }
            set
            {
                if (value.Equals(_rateDifferential)) return;
                _rateDifferential = value;
                OnPropertyChanged();
            }
        }

        public decimal CurrencyAmount
        {
            get { return _currencyAmount; }
            set
            {
                if (value.Equals(_currencyAmount)) return;
                _currencyAmount = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyError(string msg)
        {
            MessageBox.Show(msg, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
}
