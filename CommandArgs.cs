using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalfEdgeConverter
{
    public class CommandArgs
    {
        public string inputFile;
        public string outputFile;
        public int version;
        public bool binary;
        public CommandArgs()
        {
            Clear();
        }

        private bool ExplictParameter(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.First() != '-')
                {
                    throw new Exception("don't have [-] parameter");
                }

                string key = arg[1].ToString();
                string value = arg.Remove(0, 3);

                if (key == "i")
                {
                    inputFile = value;
                    if (!File.Exists(inputFile))
                    {
                        throw new Exception("Not Found InputFile Path");
                    }
                }
                else if (key == "o")
                {
                    outputFile = value;
                    if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                    {
                        throw new Exception("Not found Directory.");
                    }
                }
                else if (key == "v")
                {
                    version = int.Parse(value);
                    if (version > 2)
                    {
                        throw new Exception("not support. version = 1 or 2. ");
                    }
                }
                else if (key == "b")
                {
                    int iKey = int.Parse(value);
                    if (iKey == 0)
                    {
                        binary = false;
                    }
                    else if (iKey == 1)
                    {
                        binary = true;
                    }
                    else
                    {
                        throw new Exception("not support. binary value = 0 or 1");
                    }
                }
                else
                {
                    throw new Exception("not support format");
                }
            }

            if(binary == true && version == 1)
            {
                throw new Exception("version 1 is not support binary");
            }

            if(inputFile != string.Empty && outputFile == string.Empty)
            {
                outputFile = FileUtility.GetOutputPath(inputFile);
            }

            return true;
        }

        /// <summary>
        /// 入力ファイルの取得
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        /// <param name="inputFile">入力ファイル</param>
        /// <param name="outputFile">出力ファイル</param>
        /// <param name="version">デフォルトで1</param>
        /// <returns>Success</returns>
        public bool Parse(string[] args)
        {
            if (args.Length < 1)
            {
                Clear();
                return false;
            }

            if (args.Any(p => p.First() == '-'))
            {
                try
                {
                    return ExplictParameter(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
            else
            {
                // input, output path
                inputFile = args[0];
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine("Not Found InputFile Path");
                    Clear();
                    return false;
                }

                if (args.Length == 2)
                {
                    outputFile = args[1];
                    if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                    {
                        Console.WriteLine("Not found Directory.");
                        return false;
                    }

                    return true;
                }

                outputFile = FileUtility.GetOutputPath(inputFile);

                return true;
            }
        }

        public void ShowHelp()
        {
            Console.WriteLine("Failed Analyze CommandParameter");
            Console.WriteLine("inputFile : .stl extension only");
            Console.WriteLine("outputFile : .half extension or none");
            Console.WriteLine("version : 1 or 2, default value = 1");
            Console.WriteLine("binary : 0 or 1, default value = 0, 0 = ascii, 1 = binary; binary format is  only version 2.");
            Console.WriteLine(@"Ex. HalfEdgeConverter.exe C:\STLData.stl C:\HalfData.half");
            Console.WriteLine(@"Ex. HalfEdgeConverter.exe -i:C:\STLData.stl -o:C:\HalfData.half -v:2 -b:1");
        }

        public void Clear()
        {
            inputFile = string.Empty;
            outputFile = string.Empty;
            version = 1;
            binary = false;
        }
    }
}
