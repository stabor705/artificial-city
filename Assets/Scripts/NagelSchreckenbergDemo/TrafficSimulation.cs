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
            var numOfVertices = Math.Max(edge_start_vertices.Max(), edge_end_vertices.Max());
            for (int i = 0; i < numOfVertices; i++) {
                roadSystem.AddVertex(0, 0);
            }
            for (int i = 0; i < automatas.Count; i++) {
                roadSystem.AddEdge(automatas[i].cells.Count, edge_start_vertices[i], edge_end_vertices[i]);
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
                        automatas[i].SetOccupied(j);
                    } else {
                        automatas[i].SetEmpty(j);
                    }
                }
            }
        }
    }
}