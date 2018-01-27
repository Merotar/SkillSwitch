using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{

    public float speed;
    public Vector3 direction;
    private Vector3 origin;
    private float timeSum;

    // Use this for initialization
    void Start()
    {
        origin = gameObject.transform.position;
        timeSum = 0;
    }

    void FixedUpdate()
    {
        timeSum += Time.fixedDeltaTime;
        gameObject.transform.position = origin + direction * Mathf.Sin(timeSum * speed);
    }
}
