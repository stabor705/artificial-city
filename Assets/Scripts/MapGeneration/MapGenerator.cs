using System.Linq;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace MapGeneration {
    public class MapGenerator : MonoBehaviour {
        public TextAsset buildingsJson;
        public TextAsset roadsJson;
        public GeometryMapper geometryMapper;
        public BuildingCreator buildingCreator;
        public RoadCreator roadCreator;
        private static string MapGameObjectName = "Map";

        public void GenerateMap() {
            var map = GetNewMap();
            var buildingsDto = JsonUtility.FromJson<BuildingsDto>(buildingsJson.text);
            var buildings = buildingsDto.buildings.Select(dto => Building.FromDto(dto, geometryMapper));
            var roadsDto = JsonUtility.FromJson<RoadsDto>(roadsJson.text);
            var roads = roadsDto.network.Select(dto => Road.FromDto(dto, geometryMapper));

            foreach (var building in buildings) {
                var gameObject = buildingCreator.CreateBuilding(building.geometry);
                gameObject.transform.SetParent(map.transform);
                gameObject.name = $"Decoration {building.id}";
            }

            foreach (var road in roads) {
                var sections = road.geometry.Zip(road.geometry.Skip(1), (start, end) => (start, end)).ToList();
                foreach (var (start, end) in sections) {
                    var gameObject = roadCreator.CreateRoad(start, end);
                    gameObject.transform.SetParent(map.transform);
                    gameObject.name = $"Road {road.id}";
                }
            }
        }

        private GameObject GetNewMap() {
            var scene = EditorSceneManager.GetActiveScene();
            GameObject map = scene.GetRootGameObjects().FirstOrDefault(gameObject => gameObject.name == MapGameObjectName);
            if (map != null) {
                DestroyImmediate(map);
            }
            map = new GameObject(MapGameObjectName);
            return map;
        }
    }
}