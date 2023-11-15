// See https://aka.ms/new-console-template for more information
using NagelSchreckenbergDemo;

Console.WriteLine("Hello, World!");


var cells = new int[100];
for (int i = 0; i < cells.Length; i++)
{
    cells[i] = 0;
}

foreach (var cell in cells)
{
    Console.Write(cell);
}
Console.WriteLine("cells initialized");

var v1 = new Vehicle(1, 5, ref cells);

foreach (var cell in cells)
{
    Console.Write(cell);
}
Console.WriteLine("cells after first car");

for (int i = 0; i < cells.Length; i++)
{
    cells[i] = 0;
}
foreach (var cell in cells)
{
    Console.Write(cell);
}
Console.WriteLine("cells cleared");

var v2 = new Vehicle(1, 13, ref cells);
foreach (var cell in cells)
{
    Console.Write(cell);
}
Console.WriteLine("cells after second car");

//if ((new ArraySegment<int>(cells, 0, 13)).All(x => x == 0))
//{
//    var v2 = new Vehicle(1, 13, ref cells);
//}

