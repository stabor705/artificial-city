using UnityEditor;
using UnityEngine;

namespace MapGeneration 
{
    [CustomEditor(typeof(MapGenerator))]
    class MapGeneratorEditor : Editor {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGenerator = (MapGenerator)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Generate Map")) {
                mapGenerator.GenerateMap();
            }
        }

    }

}
