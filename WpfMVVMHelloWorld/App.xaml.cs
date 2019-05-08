using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMVVMHelloWorld
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// http://blogs.wankuma.com/kazuki/archive/2009/02/23/168586.aspx
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // ウィンドウとViewModelの初期化  
            var window = new Window1
            {
                DataContext = new HelloWorldViewModel(new Person())
            };
            window.Show();
        }
    }
}
