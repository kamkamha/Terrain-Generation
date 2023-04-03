using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
public class TerrainGeneration : MonoBehaviour
{
    ChunkGeneration chunkGen;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider collidera;
    Vector2[] uv;
    private Texture2D texture;
    private void Start()
    {
        texture = new Texture2D(128, 128);
        chunkGen = GameObject.FindGameObjectWithTag("Manager").GetComponent<ChunkGeneration>();
        if (chunkGen == null)
        {
            return;
        }
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        collidera = GetComponent<MeshCollider>();

        meshRenderer.material = chunkGen.terrainMaterial;

        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[(int)((chunkGen.octave + 1) * (chunkGen.octave + 1))];
        uv = new Vector2[vertices.Length];
        int[] triangles;

        for (int x = 0,i=0; x <= chunkGen.octave; x++)
        {
            for(int z = 0; z <= chunkGen.octave; z++)
            {
                float y = Noise(x, z);
                vertices[i] = new Vector3(x * (chunkGen.terrainSize / chunkGen.octave), y, z * (chunkGen.terrainSize / chunkGen.octave));
                if (Random.Range(0.0f, 1.0f) > chunkGen.treeThreshold && y > chunkGen.waterLevel + 4)
                {
                    float whatSpawns = Mathf.PerlinNoise(x + transform.position.x + (chunkGen.seed * 5), z + transform.position.z + (chunkGen.seed * 5));
                    whatSpawns = whatSpawns * chunkGen.tree.Length;
                    whatSpawns = Mathf.RoundToInt(whatSpawns);
                    GameObject current = Instantiate(chunkGen.tree[(int)Random.Range(0, chunkGen.tree.Length)], new Vector3(x * (chunkGen.terrainSize / chunkGen.octave) + transform.position.x, y + transform.position.y, z * (chunkGen.terrainSize / chunkGen.octave) + transform.position.z), Quaternion.identity);
                    current.transform.parent = transform;
                }
                i++;
            }
        }
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string filePath = "Assets/Model/Textures/PerlinNoise.png";
        System.IO.File.WriteAllBytes(filePath, bytes);

        for (int i =0;i< uv.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        triangles = new int[(int)(chunkGen.octave * chunkGen.octave * 6)];

        int tris = 0;
        int verts = 0;

        for(int x = 0; x< chunkGen.octave; x++)
        {
            for(int z = 0; z < chunkGen.octave; z++)
            {
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = (int)(verts + chunkGen.octave + 1);
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = (int)(verts + chunkGen.octave + 1);
                triangles[tris + 5] = (int)(verts + chunkGen.octave + 2);

                verts++;
                tris += 6;
            }
            verts++;
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateBounds();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        collidera.sharedMesh = mesh;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    float Noise(float x, float z)
    {
        //base
        float y = BiomeNoise(x, z) * 100f;
        //Mountain
        float multiplyer = 1 + Mathf.Pow(BiomeNoise(x, z),3);
        y = y * multiplyer;
        
        //float y = Mathf.PerlinNoise((((x + chunkGen.seed)* (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * chunkGen.noisevalue, (((z+chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave))+transform.position.z)* chunkGen.noisevalue) * chunkGen.amplitude;
        //y += Mathf.PerlinNoise((((x + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * (chunkGen.noisevalue*0.3f), (((z + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.z) * (chunkGen.noisevalue * 0.3f) * (chunkGen.amplitude+60));
        Color color = new Color(y / 200, y / 200, y / 200, 1);
        texture.SetPixel((int)x, (int)z, color);
        return y;
    }

    float BiomeNoise(float x,float z)
    {
        float y = Mathf.PerlinNoise((((x + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * chunkGen.noisevalue, (((z + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.z) * chunkGen.noisevalue);
        y += Mathf.PerlinNoise((((x + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * (chunkGen.noisevalue*3), (((z + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.z) * (chunkGen.noisevalue*3));
        y -= Mathf.PerlinNoise((((x + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.x) * (chunkGen.noisevalue /10* 3), (((z + chunkGen.seed) * (chunkGen.terrainSize / chunkGen.octave)) + transform.position.z) * (chunkGen.noisevalue /10* 3)) * 2;
        y = Mathf.Clamp(y, -1, 1);
        return y;
    }
    public void DestroyAll(string tag)
    {
        GameObject[] thingToDestroy = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < thingToDestroy.Length; i++)
        {
            Destroy(thingToDestroy[i]);
        }
    }
}
