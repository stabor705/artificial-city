using System;
using System.Linq;
using System.Reflection;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Edge
    {

        public Priority priority;
        public int id;
        public int length;
        public int[] cells;

        public Vertex startV;
        public Vertex endV;
        public List<Vehicle> vehicles;

        public Edge(int id, int length, Vertex startV, Vertex endV, Priority priority = Priority.MINOR)
        {
            this.id = id;
            this.length = length;
            this.cells = new int[length];
            this.startV = startV;
            this.endV = endV;
            this.vehicles = new List<Vehicle>();
            this.priority = priority;
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
                if (v.toDeleteCountdown > 0)
                    v.toDeleteCountdown--;
                if (v.toDeleteCountdown == 0)
                {
                    v.toDeleteCountdown = -1;
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
            if (new Random().NextDouble() < Configuration.VEHICLE_SPAWN_PROB)
            {
                if (this.cells.Skip(0).Take(Configuration.VEHICLE_LENGTH).Sum() == 0 && TrafficSimulation.numVehicles < Configuration.MAX_VEHICLES)
                {
                    TrafficSimulation.numVehicles++;
                    if (Configuration.VALIDATION_SCRIPT_LOGS)
                        Console.WriteLine(this.startV.ToString() + " spawning Vehicle: " + TrafficSimulation.nextVehicleIndex);
                    this.vehicles.Add(new Vehicle(Configuration.VEHICLE_LENGTH, this));
                }
            }
        }

        public void AddVehicle(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            if (Configuration.VALIDATION_SCRIPT_LOGS && vehicle.nextEdge is null)
                Console.WriteLine(this.endV.ToString() + " removing " + vehicle.ToString());
            for (int i = 0; i < this.cells.Length; i++)
                if (cells[i] == vehicle.id)
                    cells[i] = 0;

            this.vehicles.Remove(vehicle);
        }

        public override string ToString()
        {
            return string.Format("Edge: {0}", id);
        }
    }
}
