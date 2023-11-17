using System;
using System.Collections.Generic;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Vertex
    {
        public int id;
        public List<Vertex> adjacencyList;
        public List<Edge> OutEdges;
        public List<Edge> InEdges;

        public Vertex(int id, List<Vertex> adjacencyList)
        {
            this.id = id;
            this.adjacencyList = adjacencyList;
            this.OutEdges = new List<Edge>();
            this.InEdges = new List<Edge>();
        }

        public Vertex(int id)
        {
            this.id = id;
            this.adjacencyList = new List<Vertex>();
            this.OutEdges = new List<Edge>();
            this.InEdges = new List<Edge>();
        }
    }
}
