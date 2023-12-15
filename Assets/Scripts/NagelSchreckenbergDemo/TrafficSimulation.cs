using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation : MonoBehaviour
    {
        public List<CellAutomataStateManager> automatas;
        public List<int> edge_start_vertices;
        public List<int> edge_end_vertices;
        public List<Vector2> nodes;
        public List<Boolean> node_is_crossing = new List<Boolean>();
        public float tickTime;
        public uint maxSimulationFrames;
        public static int numVehicles = 0;
        private DirectedGraph.DirectedGraph roadSystem;
        private List<Vehicle> vehicles;
        private float timer = 0.0f;
        private int simulationTime = 0;
        public static int nextVehicleIndex = 1;

        void Start()
        {
            roadSystem = new DirectedGraph.DirectedGraph();

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < nodes.Count; i++) {
                if (node_is_crossing[i]) {
                    roadSystem.AddCrossing(nodes[i].x, nodes[i].y);
                } else {
                    roadSystem.AddVertex(nodes[i].x, nodes[i].y);
                }
            }
            for (int i = 0; i < automatas.Count; i++) {
                roadSystem.AddEdge(automatas[i].cells.Count, edge_start_vertices[i], edge_end_vertices[i], automatas[i].priority);
            }
        }

        void Update() {
            timer += Time.deltaTime;
            if (timer >= tickTime) {
                timer = 0.0f;
                Tick();
            }
        }

        private void Tick() {
            simulationTime++;
            if (simulationTime >= maxSimulationFrames) {
                simulationTime = 1;
            }
            foreach (var vertex in roadSystem.vertices) {
                vertex.Iterate();
            }
            foreach (var edge in roadSystem.edges) {
                edge.Iterate(simulationTime);
            }
            for (int i = 0; i < roadSystem.edges.Count; i++) {
                for (int j = 0; j < roadSystem.edges[i].cells.Length; j++) {
                    var edge = roadSystem.edges[i];
                    if (edge.cells[j] != 0) {
                        automatas[i].SetOccupied(j, edge.cells[j]);
                    } else {
                        automatas[i].SetEmpty(j);
                    }
                }
            }
        }
    }
}