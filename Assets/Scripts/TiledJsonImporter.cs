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

    private static TiledJsonImporter instance;

    void Awake()
    {
        instance = this;
    }

    public static void LoadLevel(int level)
    {
        string json = System.IO.File.ReadAllText(instance.levelPath + "/level" + level + ".json");
        instance.GenerateLevel(JsonUtility.FromJson<Level>(json));
    }

    void GenerateLevel(Level level)
    {
        Vector3 z = Vector3.zero;
        if (level.layers.Length > 2)
            z = Vector3.forward;

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
                        GameObject tile = Instantiate(prefab);
                        tile.transform.position = x * Vector3.right + (layer.height - y) * Vector3.up + z;

                    }
                }
            }
            z -= Vector3.forward;
        }
    }
}