using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    public float speed = 5;
    private Camera cam;
    public float padding = 2;
    public Vector3 offsetPerspective;
    public float offsetX;
    public float smoothingFactor = 0.005f;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float smoothing = smoothingFactor;
        if (!GameHandler.IsRunning())
            smoothing = 0.005f;
        
        Vector3 center = (Player.player1.transform.position + Player.player2.transform.position) / 2;
        Vector3 newPos = center;
        newPos.x += offsetX;
        //newPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, newPos + offsetPerspective, smoothing);

        cam.transform.LookAt(center + offsetX * Vector3.right);
    }
}
