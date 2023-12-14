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

        public TrafficSimulation(DirectedGraph.DirectedGraph roadSystem)
        {
            this.roadSystem = roadSystem;
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

        public void Run()
        {
            int time = 0;
            while (true)
            {
                time++;
                if (time >= 61)
                    time = 1;

                roadSystem.vertices.ForEach(vertex => vertex.Iterate());
                roadSystem.edges.ForEach(edge => edge.Iterate(time));

                if (Configuration.PRINT_GRAPH_STATE_ON_EACH_ITERATION)
                    PrintState();
                Thread.Sleep(Configuration.TIME_BETWEEN_ITERATIONS);
            }
        }

        public void PrintState()
        {
            // Console.WriteLine("-----------------------------------------------------------------------");
            // foreach (Vertex vertex in roadSystem.vertices)
            // {
            //     Console.WriteLine(vertex.ToString() + " state " + vertex.state);
            // }
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (Edge edge in roadSystem.edges)
            {
                Console.WriteLine(edge.ToString() + " starts from " + edge.startV.ToString() + " ends with " + edge.endV.ToString() + " and has state: " + edge.endV.GetInEdgeState(edge.id) ?? "null");
                if (Configuration.DEBUG)
                {
                    Console.WriteLine("(" + edge.startV.ToString() + ") " + string.Join("", edge.cells.Select(x =>
                    {
                        if (x == -1)
                            return '=';
                        else if (x == 0)
                            return '_';
                        else
                            return (x % 10).ToString()[0];
                    }).ToArray()) + " (" + edge.endV.ToString() + ")");
                }
                else
                {
                    Console.WriteLine("(" + edge.startV.ToString() + ") " + string.Join("", edge.cells.Select(cell =>
                    {
                        if (cell == -1)
                            return '=';
                        else if (cell == 0)
                            return '_';
                        else
                            return '#';
                    }).ToArray()) + " (" + edge.endV.ToString() + ")");
                }

            }
            Console.WriteLine("-----------------------------------------------------------------------");
        }

        public void PrintGraph()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            foreach (var vertex in roadSystem.vertices)
            {
                Console.Write(vertex.ToString() + " inbound edges from vertices: ");
                foreach (var inEdge in vertex.InEdges)
                {
                    Console.Write(inEdge.startV.ToString() + "(" + inEdge.ToString() + ") ");
                }
                Console.Write(vertex.ToString() + " outbound edges to vertices: ");
                foreach (var outEdge in vertex.OutEdges)
                {
                    Console.Write(outEdge.endV.ToString() + "(" + outEdge.ToString() + ") ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-----------------------------------------------------------------------");
        }
    }
}