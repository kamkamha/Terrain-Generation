using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkGeneration))]
public class ChunkGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChunkGeneration chunkgen = (ChunkGeneration)target;

        if (GUILayout.Button("Generate"))
        {
            TerrainGeneration[] tests = FindObjectsOfType(typeof(TerrainGeneration)) as TerrainGeneration[];
            tests[0].DestroyAll("Delete");
            foreach (var t in tests)
            {
                t.GenerateTerrain(); // do something
            }
        }
    }
}
