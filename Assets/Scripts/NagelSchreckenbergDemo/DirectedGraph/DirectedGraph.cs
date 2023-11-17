using System;
using System.Collections.Generic;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class DirectedGraph
    {
        public List<Vertex> vertices { set; get; }
        public int numVertices { set; get; }
        public List<Edge> edges { set; get; }
        public int numEdges { set; get; }

        public int[,] adjacencyMatrix { set; get; }

        public DirectedGraph()
        {
            this.vertices = new List<Vertex>();
            this.numVertices = 0;
            this.edges = new List<Edge>();
            this.numEdges = 0;

            this.adjacencyMatrix = new int[this.numVertices, this.numVertices];
        }

        // IDK why I did it
        public DirectedGraph(List<Vertex> vertices, List<Edge> edges)
        {
            this.vertices = vertices;
            this.numVertices = vertices.Count;
            this.edges = edges;
            this.numEdges = edges.Count;

            this.adjacencyMatrix = new int[this.numVertices, this.numVertices];
            foreach (var edge in edges)
            {
                adjacencyMatrix[edge.startV.id, edge.endV.id] = 1;
            }
        }

        public void AddVertex()
        {
            this.vertices.Add(new Vertex(this.numVertices));
            this.numVertices++;
        }

        public void AddEdge(int length, int startVertexId, int endVertedId)
        {
            Edge edge = new Edge(this.numEdges, length, vertices[startVertexId], vertices[endVertedId]);
            this.edges.Add(edge);
            this.numEdges++;
            this.vertices[startVertexId].OutEdges.Add(edge);
            this.vertices[endVertedId].InEdges.Add(edge);
        }
    }
}
