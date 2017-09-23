using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalfEdgeConverter.HEStructure;

namespace HalfEdgeConverter
{
    /// <summary>
    /// HalfEdgeConverter用のファイル操作を行うUtilityクラスです。
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// 出力パスを取得します。
        /// </summary>
        /// <param name="inputFile">入力ファイル</param>
        /// <param name="outputFile">出力ファイル</param>
        /// <returns>出力ファイル</returns>
        public static string GetOutputPath(string inputFile)
        {
            string directoryName = Path.GetDirectoryName(inputFile);
            string fileName = Path.GetFileNameWithoutExtension(inputFile);
            return directoryName + @"\"+ fileName + HalfEdge.FileExtension;
        }
    }
}
