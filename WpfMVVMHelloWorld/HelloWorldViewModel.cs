using System.ComponentModel;
using System.Windows.Input;

namespace WpfMVVMHelloWorld
{
    public class HelloWorldViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        // ModelクラスであるPersonを保持する。  
        // コンストラクタでModelを指定するようにしている。  
        private Person _model;

        public HelloWorldViewModel(Person model)
        {
            _model = model;
        }

        #region 入力・出力用プロパティ  

        private bool _isButtonEnable;

        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set { _isButtonEnable = value; }
        }


        // ModelクラスのNameプロパティの値の取得と設定  
        public string Name
        {
            get { return _model.Name; }
            set
            {
                if (_model.Name == value) return;
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        // こちらは通常のプロパティ  
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }
        #endregion

        #region INotifyPropertyChanged メンバ  

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IDataErrorInfo メンバ  

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                try
                {
                    if (columnName == "Name")
                    {
                        if (string.IsNullOrEmpty(this.Name))
                        {
                            return "名前を入力してください";
                        }
                    }
                    return null;
                }
                finally
                {
                    // CanExecuteChangedイベントの発行  
                    // (DelegateCommandでのCanExecuteChangedイベントで  
                    //  RequerySuggestedイベントに登録する  
                    //  処理を書いてるからこうできます)  
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        #endregion

        #region コマンド  
        private ICommand _createMessageCommand;
        public ICommand CreateMessageCommand
        {
            get
            {
                // 作成済みなら、それを返す  
                if (_createMessageCommand != null) return _createMessageCommand;

                // 遅延初期化  
                // 今回は、処理が単純なのでラムダ式で全部書いたが、通常は  
                // ViewModel内の別メソッドとして定義する。  
                _createMessageCommand = new DelegateCommand(
                    param => this.Message = string.Format("こんにちは{0}さん", this.Name),
                    param => ((IDataErrorInfo)this)["Name"] == null);
                return _createMessageCommand;
            }
        }
        #endregion
    }
}
