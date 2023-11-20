using NagelSchreckenbergDemo;
using System;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation
    {
        private DirectedGraph.DirectedGraph roadSystem;
        public static int numVehicles = 0;

        public TrafficSimulation()
        {
            roadSystem = new DirectedGraph.DirectedGraph();

            this.Initialize();
        }

        private void Initialize()
        {
            //                   6*
            //                   /|\
            //                 6  |5
            //     0     1   /<--\|  3
            //  *---->*---->*---->*---->*
            //  0     1     2  2 3|     4
            //                    |4
            //                   \|/
            //                   5*

            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();

            roadSystem.AddEdge(100, 0, 1);
            roadSystem.AddEdge(100, 1, 2);
            roadSystem.AddEdge(100, 2, 3);
            roadSystem.AddEdge(100, 3, 4);
            roadSystem.AddEdge(100, 3, 5);
            roadSystem.AddEdge(100, 3, 6);
            roadSystem.AddEdge(100, 3, 2);
        }

        public void Run()
        {
            int time = 0;
            while (true)
            {
                time++;
                if (time >= 61)
                    time = 1;

                foreach (var edge in roadSystem.edges)
                    edge.Iterate(time);

                PrintState();
                Thread.Sleep(100);
            }
        }

        public void PrintState()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (var edge in roadSystem.edges)
            {
                Console.WriteLine("Edge: " + edge.id + " starts from Vertex: " + edge.startV.id + " ends with Vertex: " + edge.endV.id);
                Console.WriteLine(string.Join("", edge.cells));
            }
            Console.WriteLine("-----------------------------------------------------------------------");
        }

        public void PrintGraph()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (var vertex in roadSystem.vertices)
            {
                Console.Write("Vertex: " + vertex.id + " inbound edges from vertices: ");
                foreach (var inEdge in vertex.InEdges)
                {
                    Console.Write(inEdge.startV.id + " ");
                }
                Console.Write("outbound edges to vertices: ");
                foreach (var inEdge in vertex.OutEdges)
                {
                    Console.Write(inEdge.endV.id + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------------------------------------------------------");
        }
    }
}