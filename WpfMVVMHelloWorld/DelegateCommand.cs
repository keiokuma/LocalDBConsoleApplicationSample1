using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMVVMHelloWorld
{
    /// <summary>  
    /// 実行する処理と、実行可能かどうかの判断を  
    /// delegateで指定可能なコマンドクラス。  
    /// </summary>  
    public class DelegateCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
        {
            _execute = executeAction;
            _canExecute = canExecuteAction;
        }

        #region ICommand メンバ  

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        // CommandManagerからイベント発行してもらうようにする  
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
