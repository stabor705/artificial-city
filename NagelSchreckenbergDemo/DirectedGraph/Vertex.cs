using System;
using System.Runtime.CompilerServices;

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

        public Vertex(int id, double lng, double lat)
        {
            this.id = id;
            this.lng = lng;
            this.lat = lat;
            this.z = -1 * (lat + lng);
            this.OutEdges = new List<Edge>();
            this.InEdges = new List<Edge>();
        }
    }

    public class Crossing : Vertex {
        public Crossing(int id, double lng, double lat) : base(id, lng, lat)
        {
            
        }
    }
    
    public class TrafficLights : Vertex {
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
