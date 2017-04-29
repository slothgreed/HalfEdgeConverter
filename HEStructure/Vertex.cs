using System;
using System.Collections.Generic;
using OpenTK;

namespace HalfEdgeConverter.HEStructure
{
    /// <summary>
    /// 頂点情報
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// 閾値
        /// </summary>
        private static float THRESHOLD05 = 0.00001f;
        /// <summary>
        /// この頂点を始点とするエッジ
        /// </summary>
        private List<Edge> m_AroundEdge = new List<Edge>();

        /// <summary>
        /// 座標
        /// </summary>
        public Vector3 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// HalfEdgeでもつm_VertexのIndex番号
        /// </summary>
        public int Index { get; set; }
        #region [operator]
        public static Vector3 operator +(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position + v2.Position);
        }
        public static Vector3 operator -(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position - v2.Position);
        }
        public static Vector3 operator *(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position * v2.Position);
        }
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            if (object.ReferenceEquals(v1, v2))
            {
                return true;
            }
            if ((object)v1 == null || (object)v2 == null)
            {
                return false;
            }

            if (Math.Abs(v1.Position.X - v2.Position.X) > THRESHOLD05)
            {
                return false;
            }
            if (Math.Abs(v1.Position.Y - v2.Position.Y) > THRESHOLD05)
            {
                return false;
            }
            if (Math.Abs(v1.Position.Z - v2.Position.Z) > THRESHOLD05)
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="number"> HalfEdgeでもつm_VertexのIndex番号</param>
        public Vertex(Vector3 pos, int index)
        {
            Position = pos;
            Index = index;
        }
        /// <summary>
        /// エッジのセッタ
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            if(!m_AroundEdge.Contains(edge))
            {
                m_AroundEdge.Add(edge);
            }
        }

        /// <summary>
        /// この頂点を始点とするエッジのゲッタ
        /// </summary>
        public IEnumerable<Edge> AroundEdge
        {
            get
            {
                if (m_AroundEdge != null)
                {
                    foreach (var edge in m_AroundEdge)
                    {
                        yield return edge;
                    }
                }
            }
        }

        /// <summary>
        /// この頂点を含むMeshの取得
        /// </summary>
        public IEnumerable<Mesh> AroundMesh
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.Mesh;
                }
            }
        }

        /// <summary>
        /// この頂点の周囲の頂点
        /// </summary>
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.End;
                }
            }
        }
    }
}
