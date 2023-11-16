using NagelSchreckenbergDemo;
using System;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation
    {
        private DirectedGraph.DirectedGraph roadSystem;
        private List<Vehicle> vehicles;
        public static int numVehicles = 0;

        public TrafficSimulation()
        {
            roadSystem = new DirectedGraph.DirectedGraph();

            this.Initialize();
        }

        private void Initialize()
        {
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();

            roadSystem.AddEdge(100, 0, 1);
            roadSystem.AddEdge(100, 1, 2);
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
                Thread.Sleep(10);
            }
        }

        public void PrintState()
        {
            foreach (var edge in roadSystem.edges)
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Edge: " + edge.id + " starts from Vertex: " + edge.startV.id + " ends with Vertex: " + edge.endV.id);
                Console.WriteLine(string.Join("", edge.cells));
                Console.WriteLine("-----------------------------------------------------------------------");
            }
        }
    }
}