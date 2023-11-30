using System;
using System.Reflection;

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

        public void AddVertex(double lng, double lat)
        {
            this.vertices.Add(new Vertex(this.numVertices, lng, lat));
            this.numVertices++;
        }

        public void AddTrafficLights(double lng, double lat)
        {
            this.vertices.Add(new TrafficLights(this.numVertices, lng, lat));
            this.numVertices++;
        }

        public void AddEdge(int length, int startVertexId, int endVertexId)
        {
            Edge edge = new Edge(this.numEdges, length, vertices[startVertexId], vertices[endVertexId]);
            this.edges.Add(edge);
            this.numEdges++;
            this.vertices[startVertexId].OutEdges.Add(edge);
            this.vertices[endVertexId].InEdges.Add(edge);

            if (this.vertices[endVertexId] is TrafficLights tl)
            {
                tl.AddTrafficLights(edge);
            }
        }
    }
}
