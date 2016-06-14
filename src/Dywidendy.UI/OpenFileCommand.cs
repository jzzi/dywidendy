using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Dywidendy.Model;

namespace Dywidendy.UI
{
    public class OpenFileCommand : ICommand
    {
        private readonly WplacViewModel _owner;
        private readonly CurrencyModel _model;

        public OpenFileCommand(WplacViewModel owner, CurrencyModel model)
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
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".dat",
            };

            var result = dlg.ShowDialog();
            if (result == true)
            {
                _owner.Events.Clear();
                var events = Helpers.GetEventsFromFile(dlg.FileName);
                _owner.Reload(events);

            }
        }

        public event EventHandler CanExecuteChanged;
    }
}