using System;

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

        public Edge(int length, Vertex startV, Vertex endV)
        {
            this.length = length;
            this.cells = new int[length];
            this.startV = startV;
            this.endV = endV;
            this.vehicles = new List<Vehicle>();
        }

        public int GetIndexOfVehicle(int id)
        {   
            Console.WriteLine("aa" + string.Join("", this.cells));
            return Array.IndexOf(this.cells, id);
        }

        public int GetSpaceInFront()
        {
            int index = 0;
            while (index < this.cells.Length && this.cells[index] == 0)
                index++;
            return index;
        }

        public void MakeMoves()
        {
            Vehicle[] tempVehicles = new Vehicle[this.vehicles.Count];
            this.vehicles.CopyTo(tempVehicles);
            
            foreach (Vehicle v in tempVehicles)
            {
                v.SingleStep();
                if (v.toDelete)
                    this.RemoveVehicle(v);
            }
        }

        public void AddVehicle(Vehicle vehicle)
        {
            this.vehicles.Add(vehicle);
        }
        
        public void RemoveVehicle(Vehicle vehicle)
        {
            Console.WriteLine("removing vehicle: " + vehicle.id);
            for (int i = 0; i < this.cells.Length; i++)
                if (cells[i] == vehicle.id)
                    cells[i] = 0;
            
            this.vehicles.Remove(vehicle);
        }
    }
}
