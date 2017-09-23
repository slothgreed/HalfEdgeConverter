using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;

namespace HEConverterUI.Behaviors
{
    /// <summary>
    /// ファイルダイアログを開くビヘイビアです。
    /// </summary>
    public class OpenFileDialogBehavior : Behavior<System.Windows.Controls.Button>
    {
        /// <summary>
        /// ファイルパスのプロパティ
        /// </summary>
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register(
                "FilePath",
                typeof(string),
                typeof(OpenFileDialogBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        /// <summary>
        /// フィルターのプロパティ
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                "Filter",
                typeof(string),
                typeof(OpenFileDialogBehavior));

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        /// <summary>
        /// フィルター
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        /// <summary>
        /// 要素にアタッチされた時の処理を行います。
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OpenFileDialogCommand;
        }

        /// <summary>
        /// 要素にデタッチされた時の処理を行います。
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Click -= OpenFileDialogCommand;
        }

        /// <summary>
        /// ファイルダイアログを開きます。
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OpenFileDialogCommand(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = Filter;
            if (FilePath != null)
            {
                dialog.InitialDirectory = Path.GetDirectoryName(FilePath);
            }

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                FilePath = dialog.FileName;
            }
        }
    }
}
