using System.Collections.Generic;
using UnityEngine;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation : MonoBehaviour
    {
        public List<CellAutomataStateManager> automatas;
        public List<Vector2> nodes;
        public List<int> edge_start_vertices;
        public List<int> edge_end_vertices;
        public List<int> edge_automata_vertices;
        public float tickTime;
        public uint maxSimulationFrames;
        public static int numVehicles = 0;
        private DirectedGraph.DirectedGraph roadSystem;
        private List<Vehicle> vehicles;
        private float timer = 0.0f;
        private int simulationTime = 0;

        void Start()
        {
            roadSystem = new DirectedGraph.DirectedGraph();

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < nodes.Count; i++) {
                roadSystem.AddVertex();
            }
            for (int i = 0; i < automatas.Count; i++) {
                roadSystem.AddEdge(automatas[i].cells.Count, edge_start_vertices[i], edge_end_vertices[i]);
            }
            // for (int i = 0; i < automatas.Count; i++) {
            //     roadSystem.AddEdge(automatas[i].cells.Count, i, i + 1);
            // }
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
            foreach (var edge in roadSystem.edges) {
                edge.Iterate(simulationTime);
            }
            for (int i = 0; i < roadSystem.edges.Count; i++) {
                for (int j = 0; j < roadSystem.edges[i].cells.Length; j++) {
                    var edge = roadSystem.edges[i];
                    if (edge.cells[j] != 0) {
                        automatas[edge_automata_vertices[i]].SetOccupied(roadSystem.edges[i].cells.Length - j - 1);
                    } else {
                        automatas[edge_automata_vertices[i]].SetEmpty(roadSystem.edges[i].cells.Length - j - 1);
                    }
                }
            }
        }
    }
}