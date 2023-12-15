using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using NagelSchreckenbergDemo;
using System.Collections.Generic;

namespace MapGeneration {
    public class MapGenerator : MonoBehaviour {
        public TextAsset buildingsJson;
        public TextAsset roadsJson;
        public GeometryMapper geometryMapper;
        public BuildingCreator buildingCreator;
        public RoadCreator roadCreator;
        public GameObject trafficSimulationPrefab;
        public GameObject mapPrefab;

        public void GenerateMap() {
            var map = GetNewMap();

            var trafficSimulation = Instantiate(trafficSimulationPrefab);
            trafficSimulation.name = "Traffic Simulation";
            trafficSimulation.transform.SetParent(map.transform);

            var buildingsDto = JsonUtility.FromJson<BuildingsDto>(buildingsJson.text);
            var buildings = buildingsDto.buildings.Select(dto => Building.FromDto(dto, geometryMapper));
            var roadsDto = JsonUtility.FromJson<RoadsDto>(roadsJson.text);
            var roads = roadsDto.network.Select(dto => Road.FromDto(dto, geometryMapper));
            List<Node> roadNodes = roadsDto.nodes.Select(dto => Node.FromDto(dto, geometryMapper)).ToList();

            foreach (var building in buildings) {
                var gameObject = buildingCreator.CreateBuilding(building.geometry);
                gameObject.transform.SetParent(map.transform);
                gameObject.name = $"Decoration {building.id}";
            }

            var trafficSimulationComponent = trafficSimulation.GetComponent<TrafficSimulation>();
            var nodes = new List<Vector2>();

            foreach (var road in roads) {
                var sections = road.geometry.Zip(road.geometry.Skip(1), (start, end) => (start, end)).ToList();
                foreach (var (start, end) in sections) {
                    var gameObject = roadCreator.CreateRoad(start, end);
                    gameObject.transform.SetParent(map.transform);
                    gameObject.name = $"Road {road.id}";

                    var leftCarSimulation = roadCreator.CreateCarSimulation(start, end, true, road.priority);
                    roadCreator.AddCarSimulationToRoad(leftCarSimulation, gameObject, start, end, true);
                    var leftCarSimulationComponent = leftCarSimulation.GetComponent<CellAutomataStateManager>();
                    var rightCarSimulation = roadCreator.CreateCarSimulation(end, start, false, road.priority);
                    roadCreator.AddCarSimulationToRoad(rightCarSimulation, gameObject, end, start, false);
                    var rightCarSimulationComponent = rightCarSimulation.GetComponent<CellAutomataStateManager>();

                    int start_node_idx = 0;
                    int end_node_idx = 0;
                    if (nodes.Contains(start)) {
                        start_node_idx = nodes.IndexOf(start);
                    } else {
                        nodes.Add(start);
                        Debug.Log(roadNodes[0].pos);
                        var roadNode = roadNodes.Find(node => node.pos == start);
                        if (roadNode != null && roadNode.isCrossing) {
                            trafficSimulationComponent.node_is_crossing.Add(true);
                        } else {
                            trafficSimulationComponent.node_is_crossing.Add(false);
                        }
                        start_node_idx = nodes.Count() - 1;
                    }

                    if (nodes.Contains(end)) {
                        end_node_idx = nodes.IndexOf(end);
                    } else {
                        nodes.Add(end);
                        var roadNode = roadNodes.Find(node => node.pos == end);
                        if (roadNode != null && roadNode.isCrossing) {
                            trafficSimulationComponent.node_is_crossing.Add(true);
                        } else {
                            trafficSimulationComponent.node_is_crossing.Add(false);
                        }
                        end_node_idx = nodes.Count() - 1;
                    }
                    trafficSimulationComponent.automatas.Add(leftCarSimulationComponent);
                    trafficSimulationComponent.edge_start_vertices.Add(start_node_idx);
                    trafficSimulationComponent.edge_end_vertices.Add(end_node_idx);
                    trafficSimulationComponent.automatas.Add(rightCarSimulationComponent);
                    trafficSimulationComponent.edge_start_vertices.Add(end_node_idx);
                    trafficSimulationComponent.edge_end_vertices.Add(start_node_idx);
                }
                trafficSimulationComponent.nodes = nodes;
            }
        }

        private GameObject GetNewMap() {
            var scene = EditorSceneManager.GetActiveScene();
            GameObject map = scene.GetRootGameObjects().FirstOrDefault(gameObject => gameObject.name == "Map");
            if (map != null) {
                DestroyImmediate(map);
            }
            map = Instantiate(mapPrefab);
            map.name = "Map";
            return map;
        }
    }
}