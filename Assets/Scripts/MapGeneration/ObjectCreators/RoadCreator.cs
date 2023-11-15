using System;
using UnityEngine;
using UnityEditor;

public class RoadCreator : MonoBehaviour {
    public GameObject roadPrefab;
    public GameObject cellPrefab;
    public GameObject carSimulationPrefab;

    private static float cellDiameter = 0.1f;
    private static float cellGap = 0.05f;

    public GameObject CreateRoad(Vector2 start, Vector2 end, GameObject map) {
        GameObject road = Instantiate(roadPrefab, map.transform);
        SpriteRenderer spriteRenderer = road.GetComponent<SpriteRenderer>();
        Vector2 roadVec = end - start;
        uint roadLength = Convert.ToUInt32(Math.Ceiling((end - start).magnitude));
        road.transform.position = start + roadVec / 2;
        spriteRenderer.size = new Vector2(roadLength, spriteRenderer.size.y);
        road.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.left, roadVec));

        var leftLane = AddCarSimulation(roadLength, road);
        leftLane.transform.Translate(new Vector3(0, 0));
        leftLane.name = "Left Lane Car Simulation";
        var rightLane = AddCarSimulation(roadLength, road);
        rightLane.transform.Translate(new Vector3(0, -0.250f));
        rightLane.name = "Right Lane Car Simulation";

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
