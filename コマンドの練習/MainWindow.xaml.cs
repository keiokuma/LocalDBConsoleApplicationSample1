using System.Windows;
using System.Windows.Input;

namespace コマンドの練習
{
    /// <summary>
    /// かずきのＢｌｏｇ　[C#][WPF]コマンドですよ その9「実行したくない時もある」
    /// http://blogs.wankuma.com/kazuki/archive/2008/03/24/129257.aspx
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 自分自身を閉じます   
            this.Close();
        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // チェックがついてたら実行可能にします   
            e.CanExecute = canClose.IsChecked ?? false;
        }
    }
}
