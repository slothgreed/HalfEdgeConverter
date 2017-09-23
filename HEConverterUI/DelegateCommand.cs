using System;
using System.Windows.Input;

namespace HEConverterUI
{
    /// <summary>
    /// コマンドのデリゲートクラス
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// コマンドの実行検証を行うイベントハンドラ
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 実行検証関数
        /// </summary>
        private Func<object, bool> canExecute;

        /// <summary>
        /// 実行関数
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="canExecute">実行検証関数</param>
        /// <param name="exeCute">実行関数</param>
        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        /// <summary>
        /// 実行検証値の変更イベント
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 実行できるかどうか
        /// </summary>
        /// <param name="parameter">コマンドパラメータ</param>
        /// <returns>実行できる</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="parameter">コマンドパラメータ</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
