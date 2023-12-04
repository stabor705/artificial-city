using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Vertex
    {
        public ushort state = 0;
        public int id;

        public double lng; // x
        public double lat; // y
        public double z;

        public List<Edge> OutEdges;
        public List<Edge> InEdges;

        public Vertex(int id, double lng, double lat)
        {
            this.id = id;
            this.lng = lng;
            this.lat = lat;
            this.z = -1 * (lat + lng);
            this.OutEdges = new List<Edge>();
            this.InEdges = new List<Edge>();
        }

        virtual public void Iterate() {}

        public bool IsAvailable(Direction direction, Priority priority)
        {
            return VertexState.IsAvailable(this.state, direction, priority);
        }
    }

    public class Crossing : Vertex
    {
        public bool pedestriansCrossing = false;
        const int MIN_COUNTDOWN = 240;
        const int COUNTDOWN_VARIANCE = 120;
        const double PROBABILITY = 0.03;

        public int countdown = -1;
        public Crossing(int id, double lng, double lat) : base(id, lng, lat)
        {

        }

        public void PedestriansCrossing()
        {
            bool safeToCross = this.InEdges.All(edge =>
            {
                var lastCells = edge.cells.TakeLast(Math.Min(edge.length, 10));
                return lastCells.All(cell => cell == 0);
            });
            if (safeToCross)
            {
                this.InEdges.ForEach(edge => edge.cells[edge.length - 1] = -1);
                this.pedestriansCrossing = true;
                this.countdown = new Random().Next(COUNTDOWN_VARIANCE) + MIN_COUNTDOWN;
            }
        }

        public void PedestriansCrossingEnd()
        {
            if (!pedestriansCrossing) return;

            foreach (Edge inEdge in this.InEdges)
            {
                inEdge.cells[inEdge.length - 1] = 0;
            }
            this.pedestriansCrossing = false;
            this.countdown = -1;
        }

        public override void Iterate()
        {
            if (this.countdown == -1)
            {
                if (new Random().NextDouble() < PROBABILITY)
                    PedestriansCrossing();
            }
            else if (this.countdown == 0)
            {
                PedestriansCrossingEnd();
            }
            else
            {
                countdown--;
            }
        }
    }

    public class TrafficLights : Vertex
    {
        public enum LightState
        {
            GREEN,
            RED
        }

        Dictionary<Edge, LightState> lights; // for every InEdge
        public TrafficLights(int id, double lng, double lat) : base(id, lng, lat)
        {
            this.lights = new Dictionary<Edge, LightState>();
            foreach (Edge inEdge in this.InEdges)
            {
                this.lights.Add(inEdge, LightState.RED);
            }
        }

        public void AddTrafficLights(Edge edge)
        {
            this.lights.Add(edge, LightState.RED);
            foreach (KeyValuePair<Edge, LightState> kvp in this.lights)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key.id, kvp.Value.ToString());
            }
        }
    }
}
