using System;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class DirectedGraph
    {
        public List<Vertex> vertices;
        public int numVertices;
        public List<Edge> edges;

        public int[,] adjacencyMatrix;

        public DirectedGraph()
        {
            this.vertices = new List<Vertex>();
            this.numVertices = 0;
            this.edges = new List<Edge>();

            this.adjacencyMatrix = new int[this.numVertices, this.numVertices];
        }

        // IDK why I did it
        public DirectedGraph(List<Vertex> vertices, List<Edge> edges)
        {
            this.vertices = vertices;
            this.numVertices = vertices.Count;
            this.edges = edges;

            this.adjacencyMatrix = new int[this.numVertices, this.numVertices];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    adjacencyMatrix[i, j] = 0;
                }
            }

            foreach (var edge in edges)
            {
                adjacencyMatrix[edge.startV.id, edge.endV.id] = 1;
            }
        }

        // public void addVertex()
        // {
        //     var vertex = new Vertex(this.numVertices)
        // }
    }
}
