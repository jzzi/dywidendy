using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;

namespace Dywidendy.UI
{
    public class SaveFileCommand : ICommand
    {
        private readonly WplacViewModel _owner;
        

        public SaveFileCommand(WplacViewModel owner)
        {
            _owner = owner;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var sfd = new SaveFileDialog {RestoreDirectory = true};

            if (sfd.ShowDialog() == true)
            {
                var toSave = _owner.Events.Select(p => string.Format(CultureInfo.InvariantCulture, "{2};{0:0.00};{1:0.0000}", p.Value, p.Rate, p.Date.ToString("yyyy-MM-dd")));
                File.WriteAllLines(sfd.FileName, toSave);
            }

        }

        public event EventHandler CanExecuteChanged;
    }
}