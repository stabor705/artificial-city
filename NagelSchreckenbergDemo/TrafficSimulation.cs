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

            this.InitializeLea();
        }

        public TrafficSimulation(DirectedGraph.DirectedGraph roadSystem)
        {
            this.roadSystem = roadSystem;
        }

        private void InitializeLea()
        {
            roadSystem.AddVertex(1, 1); // vertex 0
            roadSystem.AddCrossing(2, 1); // vertex 1
            roadSystem.AddVertex(3, 0); // vertex 2
            roadSystem.AddVertex(3, 1); // vertex 3
            roadSystem.AddVertex(3, 2); // vertex 4
            roadSystem.AddCrossing(4, 1); // vertex 5
            roadSystem.AddVertex(5, 1); // vertex 6
            roadSystem.AddCrossing(5, 2); // vertex 7
            roadSystem.AddVertex(5, 3); // vertex 8
            roadSystem.AddCrossing(6, 1); // vertex 9
            roadSystem.AddVertex(7, 0); // vertex 10
            roadSystem.AddVertex(7, 1); // vertex 11
            roadSystem.AddCrossing(8, 1); // vertex 12
            roadSystem.AddVertex(9, 1); // vertex 13
            roadSystem.AddCrossing(9, 2); // vertex 14
            roadSystem.AddVertex(9, 3); // vertex 15
            roadSystem.AddVertex(10, 1); // vertex 16

            roadSystem.AddEdge(30, 0, 1, Priority.MAJOR); // 0
            roadSystem.AddEdge(30, 1, 0, Priority.MAJOR); // 1
            roadSystem.AddEdge(10, 1, 3, Priority.MAJOR); // 2
            roadSystem.AddEdge(10, 3, 1, Priority.MAJOR); // 3
            roadSystem.AddEdge(30, 3, 2); // 4
            roadSystem.AddEdge(30, 2, 3); // 5
            roadSystem.AddEdge(30, 4, 3); // 6
            roadSystem.AddEdge(30, 3, 4); // 7
            roadSystem.AddEdge(60, 3, 5, Priority.MAJOR); // 8
            roadSystem.AddEdge(60, 5, 3, Priority.MAJOR); // 9
            roadSystem.AddEdge(10, 5, 6, Priority.MAJOR); // 10
            roadSystem.AddEdge(10, 6, 5, Priority.MAJOR); // 11
            roadSystem.AddEdge(10, 7, 6); // 12
            roadSystem.AddEdge(10, 6, 7); // 13
            roadSystem.AddEdge(20, 8, 7); // 14
            roadSystem.AddEdge(20, 7, 8); // 15
            roadSystem.AddEdge(40, 6, 9, Priority.MAJOR); // 16
            roadSystem.AddEdge(40, 9, 6, Priority.MAJOR); // 17
            roadSystem.AddEdge(10, 9, 11, Priority.MAJOR); // 18
            roadSystem.AddEdge(10, 11, 9, Priority.MAJOR); // 19
            roadSystem.AddEdge(30, 11, 10); // 20
            roadSystem.AddEdge(30, 10, 11); // 21
            roadSystem.AddEdge(40, 11, 12, Priority.MAJOR); // 22
            roadSystem.AddEdge(40, 12, 11, Priority.MAJOR); // 23
            roadSystem.AddEdge(10, 12, 13, Priority.MAJOR); // 24
            roadSystem.AddEdge(10, 13, 12, Priority.MAJOR); // 25
            roadSystem.AddEdge(10, 14, 13); // 26
            roadSystem.AddEdge(20, 15, 14); // 27
            roadSystem.AddEdge(30, 13, 16, Priority.MAJOR); // 28
            roadSystem.AddEdge(30, 16, 13, Priority.MAJOR); // 29
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