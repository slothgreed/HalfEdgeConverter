using System.Collections.ObjectModel;
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
        const int MAXVERSION = 2;
        /// <summary>
        /// InputFilePath のバッキングフィールド
        /// </summary>
        private string inputFilePath;

        /// <summary>
        /// OutputFilePath のバッキングフィールド
        /// </summary>
        private string outputFilePath;

        /// <summary>
        /// SelectedVersion のバッキングフィールド
        /// </summary>
        private int selectedVersion = 1;

        /// <summary>
        /// DoBinary のバッキングフィールド
        /// </summary>
        private bool doBinary = false;

        /// <summary>
        /// VersionList のバッキングフィールド
        /// </summary>
        private ObservableCollection<int> versionList;

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
            versionList = new ObservableCollection<int>();
            for (int i = 1; i < MAXVERSION + 1; i++)
            {
                versionList.Add(i);
            }
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
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                inputFilePath = value;
                OutputFilePath = FileUtility.GetOutputPath(inputFilePath);
                OnPropertyChanged(nameof(InputFilePath));
                ConvertCommand.OnCanExecuteChanged();
            }
        }

        public ObservableCollection<int> VersionList
        {
            get
            {
                return versionList;
            }
        }

        /// <summary>
        /// バージョン情報
        /// </summary>
        public int SelectedVersion
        {
            get
            {
                return selectedVersion;
            }

            set
            {
                selectedVersion = value;
                OnPropertyChanged(nameof(SelectedVersion));
                OnPropertyChanged(nameof(HasBinaryVersion));
            }
        }

        /// <summary>
        /// バイナリ変換するかどうか
        /// </summary>
        public bool DoBinary
        {
            get
            {
                return doBinary;
            }

            set
            {
                doBinary = value;
                OnPropertyChanged(nameof(DoBinary));
            }
        }

        public bool HasBinaryVersion
        {
            get
            {
                if (SelectedVersion == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

            halfEdge.WriteFile(OutputFilePath, SelectedVersion, DoBinary);

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
