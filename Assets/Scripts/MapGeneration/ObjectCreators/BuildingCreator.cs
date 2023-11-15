using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour {
        public GameObject buildingPrefab;
        public GameObject CreateBuilding(List<Vector2> geometry) {
            GameObject decoration = Instantiate(buildingPrefab);
            decoration.transform.position = (geometry[2] - geometry[0]) / 2 + geometry[0];
            decoration.transform.localScale = new Vector3((geometry[1] - geometry[0]).magnitude, (geometry[2] - geometry[1]).magnitude);
            decoration.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.left, geometry[0] - geometry[1]));
            return decoration;
        }
}
