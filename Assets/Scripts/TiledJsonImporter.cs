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
    public string levelPath = "teamCpp/assets/levels";

    public GameObject[] tiles;

    // Use this for initialization
    void Awake()
    {
        GenerateLevel(LoadLevel(1));
    }

    Level LoadLevel(int level)
    {
        string json = System.IO.File.ReadAllText(levelPath + "/level" + level + ".json");
        return JsonUtility.FromJson<Level>(json);
    }

    void GenerateLevel(Level level)
    {
        foreach (var layer in level.layers)
        {
            if (layer.data == null)
                continue;
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
                        float z = 0;
                        float dy = 0;
                        if (y > layer.height / 2)
                        {
                            dy = layer.height / 2;
                            z = 10;
                        }
                        GameObject tile = Instantiate(prefab);
                        Vector3 pos = new Vector3(x, layer.height / 2 - y + dy, z);
                        tile.transform.position = pos;
                    }
                }
            }
        }
    }
}