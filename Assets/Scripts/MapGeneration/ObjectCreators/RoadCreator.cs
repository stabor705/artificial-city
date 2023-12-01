using System;
using UnityEngine;
using UnityEditor;
using VectorShapes;
using NagelSchreckenbergDemo;

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


        return road;
    }

    public GameObject CreateCarSimulation(float units) {
        var numberOfCells = Convert.ToInt32(units / (cellDiameter + cellGap));
        var carSimulation = Instantiate(carSimulationPrefab);
        for (int i = 0; i < numberOfCells; i++) {
            var cell = Instantiate(cellPrefab, carSimulation.transform);
            cell.transform.Translate(new Vector3(-0.15f * i, 0));
        }
        var cellAutomata = carSimulation.GetComponent<CellAutomataStateManager>();
        cellAutomata.refreshCellList();
        return carSimulation;
    }

    public void AddCarSimulationToRoad(GameObject carSimulation, GameObject road, Vector2 start, Vector2 end, bool lane) {
        carSimulation.transform.SetParent(road.transform);
        string laneStr = lane ? "Left" : "Right";
        carSimulation.name = $"{laneStr} Lane Car Simulation";
        carSimulation.transform.position = end;
        carSimulation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(Vector3.left, start - end)));
        //var translationX = lane ? -0.5f : 0.5f;
    }
}
