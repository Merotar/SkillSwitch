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
    int[] data;
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

    // Use this for initialization
    void OnEnable()
    {
        string json = System.IO.File.ReadAllText(fileName);
        Level level = JsonUtility.FromJson<Level>(json);	
    }
}
