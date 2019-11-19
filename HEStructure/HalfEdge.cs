using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
namespace HalfEdgeConverter.HEStructure
{
    /// <summary>
    /// ハーフエッジデータ構造クラス
    /// </summary>
    public class HalfEdge
    {
        public static string FileExtension = ".half";

        /// <summary>
        /// メッシュ情報のリスト
        /// </summary>
        private List<Mesh> m_MeshList = new List<Mesh>();

        /// <summary>
        /// エッジ情報のリスト
        /// </summary>
        private List<Edge> m_EdgeList = new List<Edge>();

        /// <summary>
        /// 頂点情報のリスト
        /// </summary>
        private List<Vertex> m_VertexList = new List<Vertex>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public HalfEdge()
        {
        }

        /// <summary>
        /// ハーフエッジデータ構造の作成
        /// </summary>
        /// <param name="position">頂点情報</param>
        /// <returns>Success</returns>
        public bool Create(ReadOnlyCollection<Vector3> position)
        {
            CreateHalfEdgeData(position);
            SetOppositeEdge();
            if (!CheckOppositeEdge())
            {
                return false;
            }
            Console.WriteLine("CreateHalfEdge Process 100%");

            return true;
        }

        #region [make halfedge data structure]

        /// <summary>
        /// ハーフエッジデータ構造のために、重複する頂点情報を一つに.メッシュ情報も生成
        /// </summary>
        /// <param name="vertex_List"></param>
        private void CreateHalfEdgeData(ReadOnlyCollection<Vector3> vertex_List)
        {
            int percentage = 0;
            Vertex v1 = null, v2 = null, v3 = null;
            //最終的にポリゴンに格納する頂点
            for (int i = 0; i < vertex_List.Count; i++)
            {
                //ないVertexを調査
                Vertex vertex = m_VertexList.Find(p => p.Position == vertex_List[i]);
                if (vertex == null)
                {
                    vertex = new Vertex(vertex_List[i], m_VertexList.Count);
                    m_VertexList.Add(vertex);
                }

                if (v1 == null)
                {
                    v1 = vertex;
                }
                else if (v2 == null)
                {
                    v2 = vertex;
                }
                else
                {
                    v3 = vertex;
                    CreateMesh(v1, v2, v3);
                    v1 = null;
                    v2 = null;
                    v3 = null;
                }

                if (percentage * vertex_List.Count / 100 < i)
                {
                    Console.WriteLine("CreateHalfEdge Process" + percentage + "%");
                    percentage += 10;
                }
            }
        }

        /// <summary>
        /// メッシュの作成
        /// </summary>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        /// <param name="v3">頂点3</param>
        private void CreateMesh(Vertex v1, Vertex v2, Vertex v3)
        {
            Mesh mesh = new Mesh(m_MeshList.Count);
            Edge edge1 = new Edge(mesh, v1, v2, m_EdgeList.Count);
            Edge edge2 = new Edge(mesh, v2, v3, m_EdgeList.Count + 1);
            Edge edge3 = new Edge(mesh, v3, v1, m_EdgeList.Count + 2);

            //次のエッジの格納
            edge1.Next = edge2;
            edge2.Next = edge3;
            edge3.Next = edge1;


            edge1.Before = edge3;
            edge2.Before = edge1;
            edge3.Before = edge2;

            //頂点にエッジを持たせる
            v1.AddEdge(edge1);
            v2.AddEdge(edge2);
            v3.AddEdge(edge3);

            //メッシュにエッジを格納
            mesh.SetEdge(edge1, edge2, edge3);
            m_MeshList.Add(mesh);

            m_EdgeList.Add(edge1);
            m_EdgeList.Add(edge2);
            m_EdgeList.Add(edge3);
        }

        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach (var vertex in m_VertexList)
            {
                foreach (var edge in vertex.AroundEdge)
                {
                    SetOppositeEdgeCore(edge);
                }
            }
        }

        /// <summary>
        /// 反対エッジの取得
        /// edge      op_edge
        /// 
        ///    |      |
        /// <--・<--->・-->
        ///    |      |
        ///   
        ///       ↑
        ///  共有エッジの取得
        /// </summary>
        /// <param name="index">元となるエッジ</param>
        /// <returns></returns>
        private void SetOppositeEdgeCore(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            //反対の頂点のエッジループ
            foreach (var opposite in end.AroundEdge)
            {
                if (opposite.Start == end && opposite.End == start)
                {
                    opposite.Opposite = edge;
                    edge.Opposite = opposite;
                    break;
                }
            }
        }

        /// <summary>
        /// 反対エッジがきちんとできているかチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckOppositeEdge()
        {
            int ok_flag = 0;
            Edge opposite;
            foreach (var edge in m_EdgeList)
            {
                opposite = edge.Opposite;
                if (edge == opposite)
                {
                    ok_flag++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region [File IO]
        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="inputFile"></param>
        public bool ReadFile(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                return false;
            }
            if (Path.GetExtension(inputFile).ToLower() != FileExtension)
            {
                return false;
            }

            try
            {
                m_VertexList.Clear();
                m_EdgeList.Clear();
                m_MeshList.Clear();

                String[] fileData = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadHalfEdgeData(fileData);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("読み込み失敗 : " + inputFile);
                return false;
            }

        }

        /// <summary>
        /// ファイル内のデータをHalfEdgeで保持
        /// </summary>
        /// <param name="fileData">ファイル情報</param>
        private void ReadHalfEdgeData(String[] fileData)
        {
            int lineNumber = 0;
            String line;
            int EdgeInfoCounter = 0;
            while (fileData.Length != lineNumber)
            {
                line = fileData[lineNumber];
                lineNumber++;
                if (line.Contains("HalfEdge Data Strucure")) continue;
                if (line.Contains("Vertex :")) continue;
                if (line.Contains("Edge :")) continue;
                if (line.Contains("Mesh :")) continue;
                if (line.Contains("Edge Info :")) continue;

                string[] lineInfos = line.Split(' ');
                lineInfos = lineInfos.Where(p => !(String.IsNullOrWhiteSpace(p) || String.IsNullOrEmpty(p))).ToArray();

                if (lineInfos[0] == "v")
                {
                    var position = new Vector3(float.Parse(lineInfos[1]), float.Parse(lineInfos[2]), float.Parse(lineInfos[3]));
                    var vertex = new Vertex(position, m_VertexList.Count);
                    m_VertexList.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new Edge(m_VertexList[startIndex], m_VertexList[endIndex], m_EdgeList.Count);
                    m_EdgeList.Add(edge);
                    m_VertexList[startIndex].AddEdge(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new Mesh(m_MeshList.Count);
                    mesh.SetEdge(m_EdgeList[edge1], m_EdgeList[edge2], m_EdgeList[edge3]);
                    m_MeshList.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = m_EdgeList[EdgeInfoCounter];

                    edge.Next = m_EdgeList[nextIndex];
                    edge.Before = m_EdgeList[beforeIndex];
                    edge.Opposite = m_EdgeList[oppositeIndex];
                    edge.Mesh = m_MeshList[meshIndex];
                    EdgeInfoCounter++;
                }
            }
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="outputFile">出力ファイル</param>
        private void WriteFileVer2(string outputFile)
        {
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure V2");
            write.WriteLine(m_VertexList.Count + " " + m_EdgeList.Count + " " + m_MeshList.Count);
            foreach (var vertex in m_VertexList)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }

            foreach (var edge in m_EdgeList)
            {
                write.WriteLine("e" + " " + edge.Start.Index +" " + edge.Next.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }

            foreach (var mesh in m_MeshList)
            {
                string edgeIdx = "";
                foreach (var edge in mesh.AroundEdge)
                {
                    if (edge == mesh.AroundEdge.Last())
                    {
                        edgeIdx += edge.Index.ToString();
                    }
                    else
                    {
                        edgeIdx += edge.Index.ToString() + " ";
                    }
                }
                write.WriteLine("m" + " " + mesh.AroundEdge.First());
            }


            write.WriteLine("end");
            write.Close();
        }

        /// <summary>
        /// 書き込みバイナリ
        /// </summary>
        /// <param name="outputFile">出力ファイル</param>
        private void WriteFileVer2Binary(string outputFile)
        {
            var fileStream = File.Open(outputFile, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fileStream);
            writer.Write(2); // version;
            writer.Write(m_VertexList.Count);
            writer.Write(m_EdgeList.Count);
            writer.Write(m_MeshList.Count);

            foreach (var vertex in m_VertexList)
            {
                writer.Write(vertex.Position.X);
                writer.Write(vertex.Position.Y);
                writer.Write(vertex.Position.Z);
            }

            foreach (var edge in m_EdgeList)
            {
                writer.Write(edge.Start.Index);
                writer.Write(edge.Next.Index);
                writer.Write(edge.Opposite.Index);
                writer.Write(edge.Mesh.Index);
            }

            foreach (var mesh in m_MeshList)
            {
                writer.Write(mesh.AroundEdge.First().Index);
            }

            writer.Close();
            fileStream.Close();
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="outputFile">出力ファイル</param>
        /// <param name="version">バージョン情報</param>
        /// <param name="binary">バイナリ変換</param>
        public void WriteFile(string outputFile, int version, bool binary)
        {
            if(version == 2)
            {
                if(binary == true)
                {
                    WriteFileVer2Binary(outputFile);
                }
                else
                {
                    WriteFileVer2(outputFile);
                }
                return;
            }
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure");
            write.WriteLine("Vertex : Position");
            foreach (var vertex in m_VertexList)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }
            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in m_EdgeList)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Edge Index");
            foreach (var mesh in m_MeshList)
            {
                string edgeIdx = "";
                foreach (var edge in mesh.AroundEdge)
                {
                    if (edge == mesh.AroundEdge.Last())
                    {
                        edgeIdx += edge.Index.ToString();
                    }
                    else
                    {
                        edgeIdx += edge.Index.ToString() + " ";
                    }
                }
                write.WriteLine("m" + " " + edgeIdx);
            }

            write.WriteLine("Edge Info : Next Edge Index,Before Edge, Opposite Edge Index, Incident Face ");
            foreach (var edge in m_EdgeList)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }

            write.WriteLine("end");
            write.Close();
        }
        #endregion
    }

}
