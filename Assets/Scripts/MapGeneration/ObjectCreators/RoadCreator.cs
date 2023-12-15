using System;
using NagelSchreckenbergDemo.DirectedGraph;
using UnityEngine;
using VectorShapes;

public class RoadCreator : MonoBehaviour {
    public GameObject roadPrefab;
    public GameObject cellPrefab;
    public GameObject carSimulationPrefab;

    private static float cellDiameter = 0.1f;
    private static float cellGap = 0.05f;
    private static float scaleWidth = 0.5f;

    public GameObject CreateRoad(Vector2 start, Vector2 end) {
        GameObject road = Instantiate(roadPrefab);
        road.transform.position = Vector3.zero;

        FillRoadPolygon(road, start, end);

        return road;
    }

    private void FillRoadPolygon(GameObject road, Vector2 start, Vector2 end) {
        Shape shape = road.GetComponent<Shape>();
        shape.ShapeData.ClearPolyPoints();

        Vector2 roadVec = end - start;
        Vector2 direction = roadVec.normalized;
        Vector2 perp = new Vector2(-direction.y, direction.x) * scaleWidth;
        Vector2 A = start + perp;
        Vector2 B = start - perp;
        Vector2 C = end - perp;
        Vector2 D = end + perp;

        shape.ShapeData.AddPolyPoint(A);
        shape.ShapeData.AddPolyPoint(B);
        shape.ShapeData.AddPolyPoint(C);
        shape.ShapeData.AddPolyPoint(D);
    }

    public GameObject CreateCarSimulation(Vector2 start, Vector2 end, bool isLeft, Priority priority) {
        Vector2 roadVec = end - start;
        Vector2 direction = roadVec.normalized;
        Vector2 perp = new Vector2(-direction.y, direction.x) * scaleWidth * 0.5f;
        Vector2 A = start - perp;
        Vector2 B = end - perp;

        var m = (B.y - A.y) / (B.x - A.x);
        var c = A.y - m*A.x;
        var units = (B - A).magnitude;
        var numberOfCells = Convert.ToInt32(units / (cellDiameter + cellGap));
        var dx = (B.x - A.x) / (numberOfCells - 1);
        var carSimulation = Instantiate(carSimulationPrefab);
        for (int i = 0; i < numberOfCells; i++) {
            var cell = Instantiate(cellPrefab, carSimulation.transform);
            var x = A.x + i * dx;
            //var pos = isLeft ? new Vector3(x, m * x + c + 0.5f * scaleWidth) : new Vector3(x, m * x + c - 0.5f * scaleWidth);
            var pos = new Vector3(x, m * x + c);
            cell.GetComponent<SpriteRenderer>().transform.position = pos;
        }
        var cellAutomata = carSimulation.GetComponent<CellAutomataStateManager>();
        cellAutomata.refreshCellList();
        cellAutomata.priority = priority;
        return carSimulation;
    }

    public void AddCarSimulationToRoad(GameObject carSimulation, GameObject road, bool isLeft) {
        carSimulation.transform.SetParent(road.transform);
        string laneStr = isLeft ? "Left" : "Right";
        carSimulation.name = $"{laneStr} Lane Car Simulation";
    }
}
