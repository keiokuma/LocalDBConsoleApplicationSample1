using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFでLocalDB接続のCRUD
{
    // https://takamints.hatenablog.jp/entry/why-using-commands-in-wpf-mvvm
    public class SampleCommand : ICommand
    {
        /// <summary>
        ///忙しいフラグ。忙しい時は何もできません。
        /// </summary>
        private bool _isBusy = false;

        /// <summary>
        /// 忙しいフラグのプロパティ。
        /// コマンドが実行可能かどうかに関連するプロパティなので。
        /// 代入されたらCanExecuteChangedイベントを投げる。
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// 以下でタイマー使っているので、
        /// UIスレッドで画面更新するために必要
        /// </summary>
        private AsyncOperation _uiThreadOperation =
            AsyncOperationManager.CreateOperation(null);
        //
        // 以降 ICommand インターフェースの実装
        //

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //忙しくない時だけコマンド実行できる
            Console.WriteLine("実行可否を調べられてる。"
                + (!IsBusy ? "お仕事できます" : "今無理です"));
            return !IsBusy;
        }

        public void Execute(object parameter)
        {
            //忙しいフラグON
            //一定時間後には暇になる。
            Console.WriteLine("忙しくなるぞー");
            IsBusy = true;
            Timer _busyTimer = new Timer(timerParam => {
                _uiThreadOperation.Post(updatePropParam => {
                    IsBusy = false;
                    Console.WriteLine("暇になった。");
                }, null);
            }, null, 5000, Timeout.Infinite);
        }
    }

}
