using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainNoiseGenerate))]
public class TerrainNoiseEditor : Editor
{
    bool buttonPressed = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainNoiseGenerate terrain = (TerrainNoiseGenerate)target;

        if (GUILayout.Button("Generate"))
        {
            terrain.NoiseGen();
        }
    }
}
