using System.Collections.Generic;
using UnityEngine;
using VectorShapes;

public class BuildingCreator : MonoBehaviour {
        public GameObject buildingPrefab;

        public GameObject CreateBuilding(List<Vector2> geometry) {
            GameObject building = Instantiate(buildingPrefab);
            building.transform.position = Vector3.zero;
            Shape shape = building.GetComponent<Shape>();
            shape.ShapeData.ClearPolyPoints();
            foreach (var point in geometry) {
                shape.ShapeData.AddPolyPoint(point);
            }
            return building;
        }
}
