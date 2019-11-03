using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.ViewModel.Commands
{
    public class SearchCommand : ICommand
    {
        public WeatherVM weatherVM { get; set; }

        public event EventHandler CanExecuteChanged;

        public SearchCommand(WeatherVM weatherVM)
        {
            this.weatherVM = weatherVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            weatherVM.MakeQuery();
        }
    }
}
