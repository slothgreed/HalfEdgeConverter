using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using OpenTK;

namespace HalfEdgeConverter
{
    /// <summary>
    /// STLのローダ
    /// </summary>
    public class STLLoader
    {
        /// <summary>
        /// ファイルパス
        /// </summary>
        string FilePath;

        /// <summary>
        /// 頂点情報
        /// </summary>
        List<Vector3> vertex;

        /// <summary>
        /// 頂点情報
        /// </summary>
        public ReadOnlyCollection<Vector3> Vertex
        {
            get
            {
                return vertex.AsReadOnly();
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public STLLoader(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        /// <returns>Success</returns>
        public bool Load()
        {
            if (Path.GetExtension(FilePath).ToLower() != ".stl")
            {
                Console.WriteLine("not stl format");
                return false;
            }

            try
            {
                vertex = new List<Vector3>();
                String[] parser = File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(parser);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// STLデータの読み込み
        /// </summary>
        private void ReadData(String[] parser)
        {
            int lineCounter = 0;
            try
            {
                String[] line;
                Vector3 pos;
                while (parser.Length != lineCounter)
                {
                    line = parser[lineCounter].Split(' ');
                    line = line.Where(p => !(String.IsNullOrWhiteSpace(p) || String.IsNullOrEmpty(p))).ToArray();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == "outer" && line[i + 1] == "loop") break;
                        if (line[i] == "solid" || line[i] == "endloop" || line[i] == "endfacet") break;
                        if (line[i] == "facet" && line[i + 1] == "normal") break; // 法線は自前で計算
                        if (line[i] == "vertex")
                        {
                            pos = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                            vertex.Add(pos);
                        }

                    }
                    lineCounter++;
                }
            }
            catch (Exception)
            {
                throw new FileLoadException(lineCounter + "行目でエラー");
            }
        }

        /// <summary>
        /// STLデータの書き込み
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        public void Write(List<Vector3> position, List<Vector3> normal)
        {
            StreamWriter write = new StreamWriter("testfile.stl");
            write.WriteLine("solid stl");
            for (int i = 0; i < position.Count; i += 3)
            {
                write.WriteLine("facet normal " + normal[i].X + " " + normal[i].Y + " " + normal[i].Z);
                write.WriteLine("outer loop");

                write.WriteLine("vertex " + position[i].X + " " + position[i].Y + " " + position[i].Z);
                write.WriteLine("vertex " + position[i + 1].X + " " + position[i + 1].Y + " " + position[i + 1].Z);
                write.WriteLine("vertex " + position[i + 2].X + " " + position[i + 2].Y + " " + position[i + 2].Z);

                write.WriteLine("endloop");
                write.WriteLine("endfacet");
            }
            write.WriteLine("endsolid vcg");
            write.Close();
        }
    }
}
