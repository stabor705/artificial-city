using System;
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

    public GameObject CreateCarSimulation(Vector2 start, Vector2 end) {
        var m = (end.y - start.y) / (end.x - start.x);
        var c = start.y - m*start.x;
        var units = (end - start).magnitude;
        var numberOfCells = Convert.ToInt32(units / (cellDiameter + cellGap));
        var dx = (end.x - start.x) / numberOfCells;
        var carSimulation = Instantiate(carSimulationPrefab);
        for (int i = 0; i < numberOfCells; i++) {
            var cell = Instantiate(cellPrefab, carSimulation.transform);
            var x = start.x + i * dx;
            cell.GetComponent<SpriteRenderer>().transform.position = new Vector3(x, m * x + c);
        }
        var cellAutomata = carSimulation.GetComponent<CellAutomataStateManager>();
        cellAutomata.refreshCellList();
        return carSimulation;
    }

    public void AddCarSimulationToRoad(GameObject carSimulation, GameObject road, Vector2 start, Vector2 end, bool lane) {
        carSimulation.transform.SetParent(road.transform);
        string laneStr = lane ? "Left" : "Right";
        carSimulation.name = $"{laneStr} Lane Car Simulation";
    }
}
