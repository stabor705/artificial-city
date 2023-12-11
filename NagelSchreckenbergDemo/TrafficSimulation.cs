using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;
using System;
using System.Diagnostics;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation
    {
        private DirectedGraph.DirectedGraph roadSystem;
        public static int numVehicles = 0;
        public static int nextVehicleIndex = 1;

        public TrafficSimulation()
        {
            roadSystem = new DirectedGraph.DirectedGraph();

            this.Initialize();
        }

        private void Initialize()
        {
            //                    *5
            //                   | \      
            //                   | |
            //                  9| |8
            //                   | |
            //     7     6     5 \ | 4
            //   <---- <---- <---- <----
            //  *0    *1    *2    *3    *4
            //   ----> ----> ----> ---->
            //     0     1     2 | \ 3    
            //                   | |
            //                 10| |11
            //                   | |
            //                   \ |
            //                    *6

            const int EGDE_LENGTH = 50;

            roadSystem.AddVertex(0, 1); // vertex 0
            roadSystem.AddCrossing(1, 1); // vertex 1
            roadSystem.AddVertex(2, 1); // vertex 2
            roadSystem.AddVertex(3, 1); // vertex 3
            roadSystem.AddVertex(4, 1); // vertex 4
            roadSystem.AddVertex(3, 2); // vertex 5
            roadSystem.AddVertex(3, 0); // vertex 6

            roadSystem.AddEdge(EGDE_LENGTH, 0, 1, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 1, 2, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 2, 3, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 3, 4, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 4, 3, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 3, 2, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 2, 1, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 1, 0, Priority.MAJOR);
            roadSystem.AddEdge(EGDE_LENGTH, 3, 5);
            roadSystem.AddEdge(EGDE_LENGTH, 5, 3);
            roadSystem.AddEdge(EGDE_LENGTH, 3, 6);
            roadSystem.AddEdge(EGDE_LENGTH, 6, 3);
        }

        public void Run(bool debug = true)
        {
            int time = 0;
            while (true)
            {
                time++;
                if (time >= 61)
                    time = 1;

                roadSystem.vertices.ForEach(vertex => vertex.Iterate());
                roadSystem.edges.ForEach(edge => edge.Iterate(time));

                PrintState(debug);
                Thread.Sleep(10);
            }
        }

        public void PrintState(bool debug)
        {
            // Console.WriteLine("-----------------------------------------------------------------------");
            // foreach (Vertex vertex in roadSystem.vertices)
            // {
            //     Console.WriteLine("Vertex: " + vertex.id + " state " + vertex.state);
            // }
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (Edge edge in roadSystem.edges)
            {
                Console.WriteLine("Edge: " + edge.id + " starts from Vertex: " + edge.startV.id + " ends with Vertex: " + edge.endV.id + " and has state: " + edge.endV.GetInEdgeState(edge.id) ?? "null");
                if (debug)
                {
                    Console.WriteLine("(" + edge.startV.id + ") " + string.Join("", edge.cells.Select(x => {
                        if (x == -1)
                            return '=';
                        else if (x == 0)
                            return '_';
                        else
                            return (x % 10).ToString()[0];
                    }).ToArray()) + " (" + edge.endV.id + ")");
                }
                else
                {
                    Console.WriteLine("(" + edge.startV.id + ") " + string.Join("", edge.cells.Select(cell =>
                    {
                        if (cell == -1)
                            return '=';
                        else if (cell == 0)
                            return '_';
                        else
                            return '#';
                    }).ToArray()) + " (" + edge.endV.id + ")");
                }

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
                    Console.Write(inEdge.startV.id + "(" + inEdge.id + ") ");
                }
                Console.Write("outbound edges to vertices: ");
                foreach (var outEdge in vertex.OutEdges)
                {
                    Console.Write(outEdge.endV.id + "(" + outEdge.id + ") ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------------------------------------------------------");
        }
    }
}