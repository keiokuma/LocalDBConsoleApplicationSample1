using LocalDB接続のサンプル;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WPFでLocalDB接続のCRUD
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel _viewModel = new ViewModel();

        private string _constr;

        public string ConnectionStrings
        {
            get
            {
                if (_constr == null)
                {
                    _constr = LocalDBAdapter.GetLocalDBConnectionString(@"Database1.mdf");
                }
                return _constr;
            }
            set { _constr = value; }
        }

        string constr = null;

        public MainWindow()
        {
            InitializeComponent();
            //constr = LocalDBAdapter.GetLocalDBConnectionString(@"Database1.mdf");
            this.DataContext = _viewModel;
        }

        /// <summary>
        /// SELECTボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
            // DB表示
            _viewModel.SelectProducts(_constr);
        }


        /// <summary>
        /// UPDATEボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // 入力値の検証
            if (_viewModel.ValidateInputsUPDATE(txtId.Text, txtName.Text, txtPrice.Text))
            {
                MessageBox.Show(_viewModel.Message);
                return;
            }

            // DB更新
            _viewModel.UpdateProduct(constr, Convert.ToInt32(txtId.Text), txtName.Text, Convert.ToInt32(txtPrice.Text));
            MessageBox.Show(_viewModel.Message);
        }





        /// <summary>
        /// DELETEボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string id;
            id = txtDeleteId.Text;

            // 入力値の検証
            if (_viewModel.ValidateInputDELETE(id))
            {
                MessageBox.Show(_viewModel.Message);
                return;
            }

            // DB削除
            _viewModel.DeleteProduct(constr, Convert.ToInt32(id));
            MessageBox.Show(_viewModel.Message);
        }
    }
}
