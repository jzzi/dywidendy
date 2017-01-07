using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dywidendy.Model;
using Dywidendy.UI.Annotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dywidendy.Model.Extensions;

using DependencyAttribute = Microsoft.Practices.Unity.DependencyAttribute;

namespace Dywidendy.UI
{
    public class WplacViewModel : INotifyPropertyChanged
    {
        
        
        
        private List<Withdrawn> _withdrawns;
        private Withdrawn _lastWithdrawn;
        private ObservableCollection<IChangeDepositEvent> _events;
        private ICommand _addCommand;
        private ICommand _getCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private RateFromBankViewModel _addViewModel;
        private RateFromBankViewModel _getViewModel;
        private decimal _currencyAmount;
        private CurrencyModel _currencyModel;


        public WplacViewModel()
        {
            Reload(new List<IChangeDepositEvent>());
        }

       
        [Dependency]
        public RateFromBankViewModel AddViewModel
        {
            get { return _addViewModel; }
            set
            {
                if (Equals(value, _addViewModel)) return;
                _addViewModel = value;
                OnPropertyChanged();
            }
        }

        [Dependency]
        public RateFromBankViewModel GetViewModel
        {
            get { return _getViewModel; }
            set
            {
                if (Equals(value, _getViewModel)) return;
                _getViewModel = value;
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
               
        

        internal void Reload(IEnumerable<IChangeDepositEvent> events)
        {
            Events = new ObservableCollection<IChangeDepositEvent>(events);
            _currencyModel = new CurrencyModel(() => Events);
            AddCommand = new AddCommand(this, _currencyModel);
            GetCommand = new GetCommand(this, _currencyModel);
            OpenFileCommand = new OpenFileCommand(this, _currencyModel);
            SaveFileCommand = new SaveFileCommand(this);
            RefreshValues();
        }

        public decimal CurrencyAmount
        {
            get { return _currencyAmount; }
            set
            {
                if (value == _currencyAmount) return;
                _currencyAmount = value;
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

        public List<Withdrawn> Withdrawns
        {
            get { return _withdrawns; }
            set
            {
                if (Equals(value, _withdrawns)) return;
                _withdrawns = value;
                OnPropertyChanged();
            }
        }

        public Withdrawn LastWithdrawn
        {
            get { return _lastWithdrawn; }
            set
            {
                if (value == _lastWithdrawn) return;
                _lastWithdrawn = value;
                OnPropertyChanged();
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshValues()
        {

            CurrencyAmount = _currencyModel.CurrencyAmount();
            Withdrawns = _currencyModel.Withdrawns.ToList();
            LastWithdrawn = Withdrawns.LastOrDefault();
            Events = new ObservableCollection<IChangeDepositEvent>(_currencyModel.Events);
            
        }
    }
    
}
