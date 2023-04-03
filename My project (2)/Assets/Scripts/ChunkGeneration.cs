using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    public TerrainGeneration terrainGeneration;
    public Vector2 chunks;
    public int octave;
    [Range(0.0f, 1.0f)]
    public float noisevalue;
    public float terrainSize = 128;
    public float seed;
    public Material terrainMaterial;
    public GameObject[] tree;
    public int heightMultiplyer;
    [Range(0.0f, 1.0f)]
    public float treeThreshold;
    public GameObject water;
    public int amplitude;
    public float waterLevel;


    public float PerlinNoiseScale = 1.0f;
    public float VoronoiNoiseScale = 0;
    public float VoronoiWeight = 0;


  
    public float SimplexNoiseScale = 1.0f;
    public float SimplexWeight = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        waterLevel = Mathf.PerlinNoise(seed, seed) * amplitude;
        GameObject waters = Instantiate(water, new Vector3(( terrainSize *  chunks.x / 2),  waterLevel, ( terrainSize *  chunks.y / 2)), Quaternion.identity);
        waters.transform.localScale = new Vector3( terrainSize *  chunks.x / 5,  terrainSize *  chunks.x / 5,  terrainSize *  chunks.y / 5);
        StartCoroutine(GenerateChunks());
    }

    public IEnumerator GenerateChunks()
    {
        for (int x = 0; x < chunks.x; x++)
        {
            for (int z = 0; z < chunks.y; z++)
            {
                GameObject current = new GameObject("Terrain " + new Vector2(x * terrainSize, z * terrainSize), typeof(TerrainGeneration), typeof(MeshRenderer), typeof(MeshCollider), typeof(MeshFilter));
                current.transform.parent = transform;
                current.transform.position = new Vector3(x * (octave) * (terrainSize / octave), 0f, z * (octave) * (terrainSize / octave));
                yield return new WaitForSeconds(Time.deltaTime);

            }
        }
    }
}
