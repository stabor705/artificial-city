// See https://aka.ms/new-console-template for more information
using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;

void StateOut(DirectedGraph graph)
{
    foreach (var e in graph.edges)
    {
        Console.WriteLine("edge: " + e.id + " from " + e.startV.id + " to " + e.endV.id);
        Console.Write(string.Join("", e.cells));
        Console.WriteLine();
    }
}

void MakeMoves(DirectedGraph graph, int n)
{
    StateOut(graph);

    for (int i = 0; i < n; i++)
    {
        foreach (Edge e in graph.edges)
            e.MakeMoves();

        StateOut(graph);
    }
}

var graph = new DirectedGraph();
graph.AddVertex();
graph.AddVertex();
graph.AddVertex();

graph.AddEdge(100, 0, 1);
graph.AddEdge(100, 1, 2);

foreach (var v in graph.vertices)
{
    Console.WriteLine(v.id);
    foreach (var adj in v.adjacencyList) { Console.WriteLine("ADJ: " + adj.id); }
}

var v1 = new Vehicle(1, 5, graph.edges[0]);
// var v2 = new Vehicle(1, 13, graph.edges[1]);

// foreach (var e in graph.edges)
// {
//     Console.WriteLine("edge: " + e.id + " from " + e.startV.id + " to " + e.endV.id);
//     foreach (var c in e.cells)
//     {
//         Console.Write(c);
//     }
//     Console.WriteLine();
// }

MakeMoves(graph, 24);