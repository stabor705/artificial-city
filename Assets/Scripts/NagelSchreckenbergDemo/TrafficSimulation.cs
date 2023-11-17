using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;
using System.Collections.Generic;
using UnityEngine;

namespace NagelSchreckenbergDemo
{
    public class TrafficSimulation : MonoBehaviour
    {
        public CellAutomataStateManager automata;
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

            this.Initialize();
        }

        private void Initialize()
        {
            roadSystem.AddVertex();
            roadSystem.AddVertex();

            roadSystem.AddEdge(automata.cells.Count, 0, 1);
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
            var firstEdge = roadSystem.edges[0];
            for (int i = 0; i < firstEdge.cells.Length; i++) {
                if (firstEdge.cells[i] != 0) {
                    automata.SetOccupied(i);
                } else {
                    automata.SetEmpty(i);
                }
            }
        }
    }
}