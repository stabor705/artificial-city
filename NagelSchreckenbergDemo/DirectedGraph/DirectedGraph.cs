using System;
using System.Reflection;
using NagelSchreckenbergDemo.DirectedGraph;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class DirectedGraph
    {
        public List<Vertex> vertices { set; get; }
        public int numVertices { set; get; }
        public List<Edge> edges { set; get; }
        public int numEdges { set; get; }


        public DirectedGraph()
        {
            this.vertices = new List<Vertex>();
            this.numVertices = 0;
            this.edges = new List<Edge>();
            this.numEdges = 0;
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

        public void AddCrossing(double lng, double lat)
        {
            this.vertices.Add(new Crossing(this.numVertices, lng, lat));
            this.numVertices++;
        }

        public void AddEdge(int length, int startVertexId, int endVertexId, Priority priority = Priority.MINOR)
        {
            Edge edge = new Edge(this.numEdges, length, vertices[startVertexId], vertices[endVertexId], priority);
            this.edges.Add(edge);
            this.numEdges++;
            this.vertices[startVertexId].AddOutEdge(edge);
            this.vertices[endVertexId].AddInEdge(edge);

            if (this.vertices[endVertexId] is TrafficLights tl)
            {
                tl.AddTrafficLights(edge);
            }
        }
    }
}
