using System.ComponentModel;
using System.IO;
using System.Windows;
using HalfEdgeConverter;
using HalfEdgeConverter.HEStructure;

namespace HEConverterUI
{
    /// <summary>
    /// メインウィンドウのビューモデルクラスです。
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// InputFilePath のバッキングフィールド
        /// </summary>
        private string inputFilePath;

        /// <summary>
        /// OutputFilePath のバッキングフィールド
        /// </summary>
        private string outputFilePath;
        
        /// <summary>
        /// プロパティ変更イベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// コンバートコマンド
        /// </summary>
        public DelegateCommand ConvertCommand { get; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            ConvertCommand = new DelegateCommand(CanConvert, Convert);
        }

        /// <summary>
        /// 入力ファイルのパス
        /// </summary>
        public string InputFilePath
        {
            get
            {
                return inputFilePath;
            }

            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    return;
                }

                inputFilePath = value;
                OutputFilePath = FileUtility.GetOutputPath(inputFilePath);
                OnPropertyChanged(nameof(InputFilePath));
                ConvertCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// 出力ファイルのパス
        /// </summary>
        public string OutputFilePath
        {
            get
            {
                return outputFilePath;
            }

            set
            {
                outputFilePath = value;
                OnPropertyChanged(nameof(OutputFilePath));
            }
        }

        /// <summary>
        /// コンバートできるか
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns>実行できる</returns>
        private bool CanConvert(object parameter)
        {
            if (File.Exists(this.InputFilePath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// コンバート
        /// </summary>
        /// <param name="parameter"></param>
        private void Convert(object parameter)
        {
            STLLoader stlModel = new STLLoader(InputFilePath);

            if (!stlModel.Load())
            {
                MessageBox.Show(
                    "STLのLoadに失敗しました。",
                    "エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            HalfEdge halfEdge = new HalfEdge();
            if (!halfEdge.Create(stlModel.Vertex))
            {
                MessageBox.Show(
                    "STLのConvertに失敗しました。",
                    "エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            halfEdge.WriteFile(OutputFilePath);

            MessageBox.Show(
                    "STLのConvertに成功しました。",
                    "成功",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
        }

        /// <summary>
        /// プロパティ変更イベント
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
