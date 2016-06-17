using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dywidendy.Model;
using Dywidendy.UI.Annotations;

namespace Dywidendy.UI
{
    public class RateFromBankViewModel : INotifyPropertyChanged
    {
        private decimal _rate;
        private DateTime _date;
        private string _currency;
        private readonly NbpCurrenciesSource _currenciesService;

        public RateFromBankViewModel()
        {
            _currenciesService = new NbpCurrenciesSource();
            _currency = "EUR";
            _date = DateTime.Today;
            SetRate();
        }

        public decimal Rate
        {
            get { return _rate; }
            set
            {
                if (value == _rate) return;
                _rate = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date)) return;
                _date = value;
                OnPropertyChanged();
                SetRate();
            }
        }

        private async void SetRate()
        {
            var result = await _currenciesService.GetByDate(Currency, Date.AddDays(-1));
            if (result == null) return;
            Rate = result.Rate;
        }
        

        public string[] Currencies => new[] { "EUR", "USD", "CHF" };

        public string Currency
        {
            get { return _currency; }
            set
            {
                if (value == _currency) return;
                _currency = value;
                OnPropertyChanged();
                SetRate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}