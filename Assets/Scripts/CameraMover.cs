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
        Vector2 delta = Player.player1.transform.position - Player.player2.transform.position;
        float maxDelta = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)) + 2 * padding;

        var distance = maxDelta * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector3 center = (Player.player1.transform.position + Player.player2.transform.position) / 2;
        center.z = -distance;
        center += offset;

        //transform.position = Vector3.Lerp(cam.transform.position, center, Mathf.Atan(Time.deltaTime) * smoothingFactor);
        //center.x = transform.position.x * (1 - smoothingFactor) + smoothingFactor * center.x;

        transform.position = center;
    }
}
