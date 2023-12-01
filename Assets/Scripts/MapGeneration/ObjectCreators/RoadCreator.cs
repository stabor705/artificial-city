using System;
using UnityEngine;
using UnityEditor;
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
        Shape shape = road.GetComponent<Shape>();
        shape.ShapeData.ClearPolyPoints();

        Vector2 direction = (end - start).normalized;
        Vector2 perp = new Vector2(-direction.y, direction.x) * scaleWidth;
        Vector2 A = start + perp;
        Vector2 B = start - perp;
        Vector2 C = end - perp;
        Vector2 D = end + perp;

        Debug.Log(start);
        Debug.Log(end);
        Debug.Log(direction);
        Debug.Log(A);
        Debug.Log(B);
        Debug.Log(C);
        Debug.Log(D);

        shape.ShapeData.AddPolyPoint(A);
        shape.ShapeData.AddPolyPoint(B);
        shape.ShapeData.AddPolyPoint(C);
        shape.ShapeData.AddPolyPoint(D);

        // var leftLane = AddCarSimulation(roadLength, road);
        // leftLane.transform.Translate(new Vector3(0, 0));
        // leftLane.name = "Left Lane Car Simulation";
        // var rightLane = AddCarSimulation(roadLength, road);
        // rightLane.transform.Translate(new Vector3(0, -0.250f));
        // rightLane.name = "Right Lane Car Simulation";

        return road;
    }

    private GameObject AddCarSimulation(uint units, GameObject road) {
        var numberOfCells = Convert.ToInt32(units / (cellDiameter + cellGap));
        var carSimulation = Instantiate(carSimulationPrefab, road.transform);
        for (int i = 0; i < numberOfCells; i++) {
            var cell = Instantiate(cellPrefab, carSimulation.transform);
            cell.transform.Translate(new Vector3(-0.15f * i, 0));
        }
        var cellAutomata = carSimulation.GetComponent<CellAutomataStateManager>();
        cellAutomata.refreshCellList();
        return carSimulation;
    }
}
