using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float rotationSpeed;
    public Material material;

    void Update()
    {
        material.SetFloat("_GlowWidth", Mathf.Sin(Time.time * rotationSpeed));
    }
}
