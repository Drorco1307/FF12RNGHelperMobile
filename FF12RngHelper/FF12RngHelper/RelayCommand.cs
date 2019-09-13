using System;
using System.Windows.Input;

namespace FF12RngHelper
{
    public class RelayCommand : ICommand
    {
        private Action m_action;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            m_action();
        }

        public RelayCommand(Action action)
        {
            m_action = action;
        }
    }
}
