using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFでLocalDB接続のCRUD
{
    // https://stackoverflow.com/questions/12422945/how-to-bind-wpf-button-to-a-command-in-viewmodelbase
    public class CommandHandler : ICommand
    {
        private Action _execute;
        private bool _canExecute;
        //private Action<object> _execute;
        //private Func<object, bool> _canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            _execute = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action action): this(action, true)
        {
            _execute = action;
        }

        public static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            //return _canExecute == null ? true : _canExecute(parameter);
            return _canExecute;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }


        public void Execute(object parameter)
        {
            //if (_execute != null)
            //    _execute(parameter);
            _execute();
        }
    }
}
