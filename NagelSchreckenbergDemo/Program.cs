// See https://aka.ms/new-console-template for more information
using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;

const bool DEBUG = true;

TrafficSimulation simulator  = new TrafficSimulation();
simulator.PrintGraph();
simulator.Run(DEBUG);