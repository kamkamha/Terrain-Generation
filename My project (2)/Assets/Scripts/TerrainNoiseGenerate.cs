using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNoiseGenerate : MonoBehaviour
{
    [Range(0f, 1f)]
    public float noiseValue;
    public int seed;
    public Terrain terrain;
    public float terrainSize = 2000.0f;
    public float terrainHeight = 600.0f;
    void Start()
    {
        NoiseGen();
    }
    private void Update()
    {
 
    }
    public void NoiseGen()
    {
        // Initialize the corner points of the height map
        //heightMap[0, 0] = 0;
        //heightMap[0, size - 1] = 0;
        //heightMap[size - 1, 0] = 0;
        //heightMap[size - 1, size - 1] = 0;
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);
        terrainData.size = terrainData.size;
        float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                float height = Noise(x,z) * noiseValue;
                heights[z, x] = height;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    float Noise(float x, float z)
    {
        //base
        float y = BiomeNoise(x, z) * 100f;
        //Mountain
        float multiplyer = 1 + Mathf.Pow(BiomeNoise(x, z), 3);
        y = y * multiplyer;

        //float y = Mathf.PerlinNoise((((x + chunkGen.seed)* (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * chunkGen.noisevalue, (((z+chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave))+transform.position.z)* chunkGen.noisevalue) * chunkGen.amplitude;
        //y += Mathf.PerlinNoise((((x + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * (chunkGen.noisevalue*0.3f), (((z + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.z) * (chunkGen.noisevalue * 0.3f) * (chunkGen.amplitude+60));

        return y;
    }

    float BiomeNoise(float x, float z)
    {
        float y = Mathf.PerlinNoise((x + seed) * noiseValue, (z + seed) * noiseValue);
        y += Mathf.PerlinNoise((x + seed) * noiseValue * 3, (z + seed) * noiseValue * 3);
        y -= Mathf.PerlinNoise((x + seed) * noiseValue / 10 * 3, (z + seed) * noiseValue / 10 * 3) * 2;
        y = Mathf.Clamp(y, -1, 1);
        return y;
    }
}

