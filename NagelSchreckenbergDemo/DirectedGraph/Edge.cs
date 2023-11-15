using System;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Edge
    {
        public int length;
        public int[] cells;

        public Vertex startV;
        public Vertex endV;

        public Edge(int length, Vertex startV, Vertex endV)
        {
            this.length = length;
            this.cells = new int[length];
            this.startV = startV;
            this.endV = endV;
        }
    }
}
