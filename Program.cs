using System;
using System.IO;
using HalfEdgeConverter.HEStructure;
namespace HalfEdgeConverter
{
    class Program
    {

        /// <summary>
        /// 入力ファイルの取得
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        /// <param name="inputFile">入力ファイル</param>
        /// <param name="outputFile">出力ファイル</param>
        /// <returns>Success</returns>
        static bool GetInOutFile(string[] args, out string inputFile, out string outputFile)
        {
            if (args.Length < 1)
            {
                inputFile = null;
                outputFile = null;
                return false;
            }
            inputFile = args[0];
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Not Found InputFile Path");
                inputFile = null;
                outputFile = null;
                return false;
            }

            string directoryName = Path.GetDirectoryName(inputFile);
            string fileName = Path.GetFileNameWithoutExtension(inputFile);

            if (args.Length == 1)
            {
                outputFile = directoryName + fileName + HalfEdge.FileExtension;
            }
            else
            {
                outputFile = args[1];
                if(!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Console.WriteLine("Not found Directory.");
                    return false;
                }
            }
            return true;
        }
        static void Main(string[] args)
        {
            string inputFile;
            string outputFile;

            if (!GetInOutFile(args, out inputFile, out outputFile))
            {
                Console.WriteLine("Failed Analyze CommandParameter");
                Console.WriteLine("inputFile : .stl extension only");
                Console.WriteLine("outputFile : .half extension or none");
                Console.WriteLine(@"Ex. HalfEdgeConverter.exe C:\STLData.stl C:\HalfData.half");
                return;
            }

            Console.WriteLine("Loading STL Data : " + inputFile);
            STLLoader stlModel = new STLLoader(inputFile);
            if (!stlModel.Load())
            {
                Console.WriteLine("Failed Load STL Data");
                return;
            }
            Console.WriteLine("Loaded STL Data");
            
            
            HalfEdge half = new HalfEdge();
            Console.WriteLine("Create Half Edge Data");
            if (!half.Create(stlModel.Vertex))
            {
                Console.WriteLine("Failed HalfEdge Structure");
                return;
            }
            Console.WriteLine("Created HalfEdge Data");

            Console.WriteLine("Output File : " + outputFile);
            half.WriteFile(outputFile);
            Console.WriteLine("Success");
            return;
        }
    }
}
