using System;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Vertex
    {
        public int id;
        public Vertex[] adjacencyList;

        public Vertex(int id, Vertex[] adjacencyList)
        {
            this.id = id;
            this.adjacencyList = adjacencyList;
        }
    }
}
