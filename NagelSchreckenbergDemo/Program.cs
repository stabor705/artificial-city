// See https://aka.ms/new-console-template for more information
using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;

TrafficSimulation simulator  = new TrafficSimulation();
simulator.Iterate(30);
//simulator.RealIterate();