using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    public float speed = 5;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }
	
    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(Time.time * speed, cam.transform.position.y, cam.transform.position.z); 
    }
}
