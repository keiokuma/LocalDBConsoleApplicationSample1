using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace WPFでLocalDB接続のCRUD
{
    public partial class ViewModel : ViewModelBase, IDataErrorInfo
    {
        // ModelクラスであるProductを保持する。  
        // コンストラクタでModelを指定するようにしている。  
        private Product _model;
        private bool _canExecute;


        public ViewModel()
        {
            _model = new Product();
            _model.Result = "Sample";
            _canExecute = true;
        }

        private SampleCommand _sampleCommand;

        public SampleCommand SampleCommand
        {
            get
            {
                if (_sampleCommand == null)
                {
                    _sampleCommand = new SampleCommand();
                }
                return _sampleCommand;
            }
            set
            {
                _sampleCommand = value;
            }
        }

        #region Properties

        public int ID
        {
            get
            {
                return _model.Id;
            }
            set
            {
                if (_model.Id == value)
                {
                    return;
                }
                _model.Id = value;
                NotifyPropertyChange("ID");
            }
        }


        public string Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                if (_model.Name == value)
                {
                    return;
                }
                _model.Name = value;
                NotifyPropertyChange("Name");
            }
        }


        public int Price
        {
            get
            {
                return _model.Price;
            }
            set
            {
                if (_model.Price == value)
                {
                    return;
                }
                _model.Price = value;
                NotifyPropertyChange("Price");
            }
        }


        public string Result
        {
            get
            {
                return _model.Result;
            }
            set
            {
                if (_model.Result == value)
                {
                    return;
                }
                _model.Result = value;
                NotifyPropertyChange("Result");
            }
        }


        public string Message
        {
            get
            {
                return _model.Message;
            }
            set
            {
                if (_model.Message == value)
                {
                    return;
                }
                _model.Message = value;
                NotifyPropertyChange("Message");
            }
        }




        #endregion

        #region DataNotification

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
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

        #region Commands

        private ICommand _selectCommand;

        public ICommand SelectCommand
        {
            get { return _selectCommand; }
            set { _selectCommand = value; }
        }

        private ICommand _updateCommand;

        public ICommand UpdateCommand
        {
            get { return _updateCommand; }
            set { _updateCommand = value; }
        }

        private ICommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { _deleteCommand = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// SELECT実行
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public void SelectProducts(string constr)
        {
            Result = null;

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Result += string.Format("{0}:{1}:{2}", reader["Id"], reader["Name"], reader["Price"]);
                        Result += Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    Message = "SELECTに失敗しました。";
                    throw ex;
                }
            }

            Message = "SELECTしました。";
        }

        internal void UpdateProduct(string constr, int id, string name, int price)
        {
            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;

                command.CommandText =
                "IF NOT EXISTS(SELECT 1 from Products where Id=@Id)" +
                " Insert INTO Products(Id, Name, Price) VALUES(@Id,@Name,@Price)" +
                " else" +
                " UPDATE Products SET Id=@Id,Name=@Name,Price=@Price WHERE Id=@Id";

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Price", price);

                try
                {
                    connection.Open();
                    int num = command.ExecuteNonQuery();
                    if (num == -1)
                    {
                        Message = "UPDATEに失敗しました。";

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                Message = "UPDATEに成功しました。";
            }
        }

        internal bool ValidateInputsUPDATE(string id, string name, string price)
        {
            // 入力値の検証
            if (string.IsNullOrWhiteSpace(id))
            {
                Message = "IDを入力してください。";
                return true;
            }

            int output;
            if (!Int32.TryParse(id, out output))
            {
                if (output == 0)
                {
                    Message = "IDを半角数字で入力してください。";
                    return true;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Message = "Nameを入力してください。";
                return true;
            }

            if (string.IsNullOrWhiteSpace(price))
            {
                Message = "Priceを入力してください。";
                return true;
            }

            if (!Int32.TryParse(price, out output))
            {
                if (output == 0)
                {
                    Message = "Priceを半角数字で入力してください。";
                    return true;
                }
            }

            Message = null;
            return false;
        }


        internal bool ValidateInputDELETE(string id)
        {
            // 入力値の検証
            if (string.IsNullOrWhiteSpace(id))
            {
                Message = "IDを入力してください。";
                return true;
            }

            int output;
            if (!Int32.TryParse(id, out output))
            {
                if (output == 0)
                {
                    Message = "IDを半角数字で入力してください。";
                    return true;
                }
            }

            Message = "";
            return false;
        }

        internal void DeleteProduct(string constr, int id)
        {
            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;


                command.CommandText =
                "IF EXISTS(SELECT 1 from Products where Id=@Id)" +
                " DELETE Products WHERE Id=@Id";

                command.Parameters.AddWithValue("@Id", id);


                try
                {
                    connection.Open();
                    int num = command.ExecuteNonQuery();
                    if (num == -1)
                    {
                        Message = "DELETEに失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }

                Message = "DELETEに成功しました。";
            }
        }

        #endregion

    }
}
