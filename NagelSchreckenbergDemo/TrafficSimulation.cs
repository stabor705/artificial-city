using NagelSchreckenbergDemo;
using System;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation
    {
        private DirectedGraph.DirectedGraph roadSystem;
        private List<Vehicle> vehicles;

        public TrafficSimulation()
        {
            roadSystem = new DirectedGraph.DirectedGraph();
            vehicles = new List<Vehicle>();

            this.Initialize();
        }

        private void Initialize()
        {
            roadSystem.AddVertex();
            roadSystem.AddVertex();
            roadSystem.AddVertex();

            roadSystem.AddEdge(100, 0, 1);
            roadSystem.AddEdge(100, 1, 2);

            vehicles.Add(new Vehicle(1, 5, roadSystem.edges[0]));
        }

        private void Update()
        {
            foreach (var edge in roadSystem.edges)
            {
                edge.MakeMoves();
            }
        }

        public void RealIterate()
        {
            int time = 0;
            while (true)
            {
                time++;
                if (time >= 61)
                    time = 1;

                foreach (var vehicle in vehicles)
                {
                    if ((vehicle.velocity * time % 60) == 0)
                    {
                        vehicle.SingleStep();
                        if (vehicle.toDelete)
                            vehicle.edge.RemoveVehicle(vehicle);
                        this.PrintState();
                    }
                }
                Thread.Sleep(3);
            }
        }

        public void Iterate(int nIterations)
        {
            for (int i = 0; i < nIterations; i++)
            {
                Console.WriteLine("Iteration: " + (i + 1));
                this.Update();
                this.PrintState();
                Console.WriteLine("");
                // Thread.Sleep(1000);
            }
        }

        public void PrintState()
        {
            foreach (var edge in roadSystem.edges)
            {
                Console.WriteLine("Edge: " + edge.id + " starts from Vertex: " + edge.startV.id + " ends with Vertex: " + edge.endV.id);
                Console.WriteLine(string.Join("", edge.cells));
            }
        }
    }
}