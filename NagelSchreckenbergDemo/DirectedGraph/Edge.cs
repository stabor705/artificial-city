using System;
using System.Linq;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Edge
    {
        public int id;
        public int length;
        public int[] cells;

        public Vertex startV;
        public Vertex endV;
        public List<Vehicle> vehicles;

        public Edge(int id, int length, Vertex startV, Vertex endV)
        {
            this.id = id;
            this.length = length;
            this.cells = new int[length];
            this.startV = startV;
            this.endV = endV;
            this.vehicles = new List<Vehicle>();
        }

        public int GetIndexOfVehicle(int id)
        {   
            return Array.IndexOf(this.cells, id);
        }

        public int GetSpaceInFront()
        {
            int index = 0;
            while (index < this.cells.Length && this.cells[index] == 0)
                index++;
            return index;
        }

        public void Iterate(int time)
        {
            Vehicle[] tempVehicles = new Vehicle[this.vehicles.Count];
            this.vehicles.CopyTo(tempVehicles);

            foreach (Vehicle v in tempVehicles)
            {
                if (v.toDelete)
                    this.RemoveVehicle(v);
                    TrafficSimulation.numVehicles--;
                }
                v.SingleStep(time);
            }

            if (startV.OutEdges.Count == 1 && // because we want to get out of that vertex and we don't want to end up on crossroads
                (startV.InEdges is null ||
                startV.InEdges.Count == 0 || // it may be uni-directional road
                (startV.InEdges.Count == 1 && startV.InEdges.First().startV == this.endV)) // or it can be bi-directional
               )
                SpawnVehicle();
        }

        private void SpawnVehicle()
        {
            if (new Random().NextDouble() < 0.1)
            {
                int vehicleLength = 5;
                if (this.cells.Skip(0).Take(vehicleLength).Sum() == 0 && TrafficSimulation.numVehicles < 3)
                {
                    TrafficSimulation.numVehicles++;
                    Console.WriteLine("Spawning vehicle " + TrafficSimulation.numVehicles);
                    this.vehicles.Add(new Vehicle(vehicleLength, this));
                }
            }
        }

        public void AddVehicle(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle);
        }
        
        public void RemoveVehicle(Vehicle vehicle)
        {
            Console.WriteLine("removing vehicle: " + vehicle.id + " from edge: " + this);
            for (int i = 0; i < this.cells.Length; i++)
                if (cells[i] == vehicle.id)
                    cells[i] = 0;
            
            this.vehicles.Remove(vehicle);
        }

        public override string ToString()
        {
            return string.Format("Edge {0}", id);
        }
    }
}
