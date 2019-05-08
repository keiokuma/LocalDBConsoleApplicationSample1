using System.Windows;
using System.Windows.Input;

namespace WPFでLocalDB接続のCRUD
{
    public partial class ViewModel
    {
        private ICommand _clickCommand;

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), _canExecute));
            }
        }

        public void MyAction()
        {
            MessageBox.Show("AAA", "BBB");
        }

        private DelegateCommand clearCommand;
        public DelegateCommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                    clearCommand = new DelegateCommand(
                    _ => this.Message = string.Empty,
                    _ => !string.IsNullOrEmpty(this.Message)
                    );
                return clearCommand;
            }
        }
    }
}
