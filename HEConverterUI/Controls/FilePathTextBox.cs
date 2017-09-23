using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HEConverterUI
{
    /// <summary>
    /// ファイルパス用のテキストボックスです。
    /// </summary>
    public class FilePathTextBox : TextBox
    {
        /// <summary>
        /// ファイルパスのプロパティ
        /// </summary>
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register(
                "FilePath",
                typeof(string),
                typeof(FilePathTextBox),
                new PropertyMetadata(null, OnFilePathChanged));

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FilePathTextBox()
        {
            IsReadOnly = true;

            ToolTipService.SetInitialShowDelay(this, 0);
        }

        /// <summary>
        /// ファイルパスが変更されたときに呼び出されます。
        /// </summary>
        /// <param name="d">FlePathTextBox</param>
        /// <param name="e">イベント</param>
        public static void OnFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as FilePathTextBox;

            if (textBox != null)
            {
                textBox.OnFilePathChanged((string)e.NewValue);
            }
        }

        /// <summary>
        /// ファイルパスが変更されたときの処理を行います。
        /// </summary>
        /// <param name="d">FlePathTextBox</param>
        /// <param name="e">イベント</param>
        public void OnFilePathChanged(string newValue)
        {

            if (string.IsNullOrEmpty(newValue))
            {
                this.ToolTip = null;
            }
            else
            {
                this.ToolTip = newValue;
            }

            if (this.IsFocused)
            {
                this.Text = newValue;
            }
            else
            {
                this.Text = Path.GetFileName(newValue);
            }
        }

        /// <summary>
        /// フォーカスが得られた時の処理を行います。
        /// </summary>
        /// <param name="e">イベント</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            this.Text = FilePath;
        }

        /// <summary>
        /// フォーカスが失われた時の処理を行います。
        /// </summary>
        /// <param name="e">イベント</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this.Text = Path.GetFileName(FilePath);
        }
    }
}
