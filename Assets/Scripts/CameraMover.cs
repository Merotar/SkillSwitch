using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    public float speed = 5;
    private Camera cam;
    public float padding = 2;
    public Vector3 offset;
    public float smoothingFactor = 0.5f;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = (Player.player1.transform.position + Player.player2.transform.position) / 2;
        Vector3 newPos = transform.position;
        newPos.x = center.x;
        transform.position = newPos + offset;
        cam.transform.LookAt(center);
    }
}
