using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    public int height;
    public bool infinite;
    public Layer[] layers;
}

[Serializable]
public class Layer
{
    public int[] data;
    public int height;
    public string name;
    public float opacity;
    public string type;
    public bool visible;
    public int width;
    public int x;
    public int y;
}

public class TiledJsonImporter : MonoBehaviour
{
    public string fileName = "level.json";
    public static Level importedLevel;

    public GameObject[] tiles;

    // Use this for initialization
    void Awake()
    {
        string json = System.IO.File.ReadAllText(fileName);
        importedLevel = JsonUtility.FromJson<Level>(json);
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector3 z = Vector3.zero;
        foreach (var layer in importedLevel.layers)
        {
            int width = layer.data.Length / layer.height;
            for (int y = 0; y != layer.height; ++y)
            {
                for (int x = 0; x != width; ++x)
                {
                    int index = x + width * y;
                    int objId = layer.data[index];
                    GameObject prefab = tiles[objId];
                    if (prefab != null)
                    {
                        GameObject tile = Instantiate(prefab);
                        tile.transform.position = x * Vector3.right + y * Vector3.up + z;

                    }
                }
            }
            z -= Vector3.forward;
        }
    }
}