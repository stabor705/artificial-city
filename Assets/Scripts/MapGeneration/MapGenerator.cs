using System.Linq;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace MapGeneration {
    public class MapGenerator : MonoBehaviour {
        public TextAsset decorationsFile;
        public BuildingCreator buildingCreator;
        public RoadCreator roadCreator;
        private static string MapGameObjectName = "Map";

        public void GenerateMap() {
            var map = GetNewMap();
            var objectsDto = JsonUtility.FromJson<ObjectsDto>(decorationsFile.text);
            var decorations = objectsDto.decorations.Select(dto => Decoration.FromDto(dto)).ToList();
            Debug.Log(decorations);
            var roads = objectsDto.roads.Select(dto => Road.FromDto(dto)).ToList();

            foreach (var decoration in decorations) {
                var gameObject = buildingCreator.CreateBuilding(decoration.geometry);
                gameObject.transform.SetParent(map.transform);
                gameObject.name = $"Decoration {decoration.id}";
            }

            foreach (var road in roads) {
                var sections = road.geometry.Zip(road.geometry.Skip(1), (start, end) => (start, end)).ToList();
                foreach (var (start, end) in sections) {
                    var gameObject = roadCreator.CreateRoad(start, end, map);
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