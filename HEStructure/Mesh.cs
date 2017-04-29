using System.Collections.Generic;
namespace HalfEdgeConverter.HEStructure
{
    /// <summary>
    /// メッシュ
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// メッシュを構成するエッジ
        /// </summary>
        private List<Edge> m_Edge = new List<Edge>();

        /// <summary>
        /// HalfEdgeでもつm_MeshのIndex番号
        /// </summary>
        public int Index
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="index"></param>
        public Mesh(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="edge1">メッシュを構成するエッジ1</param>
        /// <param name="edge2">メッシュを構成するエッジ2</param>
        /// <param name="edge3">メッシュを構成するエッジ3</param>
        /// <param name="index">HalfEdgeでもつm_MeshのIndex番号</param>
        public Mesh(Edge edge1, Edge edge2, Edge edge3, int index)
        {
            SetEdge(edge1, edge2, edge3);
            Index = index;
        }

        /// <summary>
        /// setter Edge
        /// </summary>
        /// <param name="edge1">メッシュを構成するエッジ1</param>
        /// <param name="edge2">メッシュを構成するエッジ2</param>
        /// <param name="edge3">メッシュを構成するエッジ3</param>
        public void SetEdge(Edge edge1, Edge edge2, Edge edge3)
        {
            m_Edge.Clear();
            m_Edge.Add(edge1);
            m_Edge.Add(edge2);
            m_Edge.Add(edge3);
        }

        /// <summary>
        /// Meshを構成するエッジのゲッタ
        /// </summary>
        public IEnumerable<Edge> AroundEdge
        {
            get
            {
                return m_Edge;
            }
        }

        /// <summary>
        /// メッシュを構成する頂点のゲッタ
        /// </summary>
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in m_Edge)
                {
                    yield return edge.Start;
                }
            }
        }
    }
}
