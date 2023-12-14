using System;
using System.Collections.Generic;
using System.Linq;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public class Vertex
    {
        public int id;

        public double lng; // x
        public double lat; // y
        public double z;

        public List<Edge> OutEdges;
        public List<Edge> InEdges;
        public Dictionary<int, ushort> inEdgeStates;

        public Vertex(int id, double lng, double lat)
        {
            this.id = id;
            this.lng = lng;
            this.lat = lat;
            this.z = -1 * (lat + lng);
            this.OutEdges = new List<Edge>();
            this.InEdges = new List<Edge>();
            this.inEdgeStates = new Dictionary<int, ushort>();
        }

        virtual public void Iterate() {}

        public void AddOutEdge(Edge edge)
        {
            this.OutEdges.Add(edge);
        }

        public void AddInEdge(Edge edge)
        {
            this.InEdges.Add(edge);
            inEdgeStates.Add(edge.id, 0);
        }

        public ushort GetInEdgeState(int edgeId)
        {
            return this.inEdgeStates[edgeId];
        }

        public void SetInEdgeState(int edgeId, ushort stateToSet)
        {
            this.inEdgeStates[edgeId] = VertexState.SetState(
                this.inEdgeStates[edgeId],
                stateToSet
            );
        }

        public void UnsetInEdgeState(int edgeId, ushort stateToUnset)
        {
            this.inEdgeStates[edgeId] = VertexState.UnsetState(
                this.inEdgeStates[edgeId],
                stateToUnset
            );
        }

        public bool IsAvailable(Direction direction, Priority priority)
        {
            return VertexState.IsAvailable(
                this.inEdgeStates.Values.Aggregate((result, next) => (ushort)(result | next)), 
                direction, 
                priority
            );
        }

        public override string ToString()
        {
            return string.Format("Vertex: {0}", id);
        }
    }

    public class Crossing : Vertex
    {
        public bool pedestriansCrossing = false;
        public int countdown = -1;

        public Crossing(int id, double lng, double lat) : base(id, lng, lat) {}

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
                if (Configuration.VALIDATION_SCRIPT_LOGS)
                    Console.WriteLine(this.ToString() + " pedestrians crossing start");
                this.countdown = new Random().Next(Configuration.PEDESTRIAN_CROSSING_COUNTDOWN_VARIANCE) + Configuration.PEDESTRIAN_CROSSING_MIN_COUNTDOWN;
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
            if (Configuration.VALIDATION_SCRIPT_LOGS)
                Console.WriteLine(this.ToString() + " pedestrians crossing end");
            this.countdown = -1;
        }

        public override void Iterate()
        {
            if (this.countdown == -1)
            {
                if (new Random().NextDouble() < Configuration.PEDESTRIAN_CROSSING_PROBABILITY)
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
