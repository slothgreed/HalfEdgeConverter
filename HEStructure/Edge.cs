namespace HalfEdgeConverter.HEStructure
{
    /// <summary>
    /// エッジ
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// 始点
        /// </summary>
        public Vertex Start { get; set; }
        /// <summary>
        /// 終点
        /// </summary>
        public Vertex End { get; set; }

        /// <summary>
        /// メッシュ
        /// </summary>
        public Mesh Mesh { get; set; }
        /// <summary>
        /// 次のエッジ
        /// </summary>
        public Edge Next { get; set; }

        /// <summary>
        /// 前のエッジ
        /// </summary>
        public Edge Before { get; set; }

        /// <summary>
        /// 反対エッジ
        /// </summary>
        public Edge Opposite { get; set; }

        /// <summary>
        /// 初期のIndex
        /// </summary>
        public int Index { get; set; }

        public static bool operator ==(Edge edge1, Edge edge2)
        {
            //参照が同じならTrue
            if (object.ReferenceEquals(edge1, edge2))
            {
                return true;
            }
            if ((object)edge1 == null || (object)edge2 == null)
            {
                return false;
            }
            //共有EdgeでもTrue
            return (edge1.Start == edge2.End && edge1.End == edge2.Start);
        }
        public static bool operator !=(Edge edge1,Edge edge2)
        {
            return !(edge1 == edge2);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mesh">エッジを持つ面</param>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">HalfEdgeでもつm_EdgeのIndex番号</param>
        public Edge(Mesh mesh, Vertex start, Vertex end, int index)
        {
            Mesh = mesh;
            Start = start;
            End = end;
            Index = index;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <param name="index">HalfEdgeでもつm_EdgeのIndex番号</param>
        public Edge(Vertex start, Vertex end, int index)
        {
            Start = start;
            End = end;
            Index = index;
        }

    }
}
