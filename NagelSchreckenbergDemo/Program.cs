using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;

Configuration.DisplayConfiguration();

TrafficSimulation simulator  = new TrafficSimulation();

if (Configuration.PRINT_GRAPH_STRUCTURE)
    simulator.PrintGraph();

simulator.Run();