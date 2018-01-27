using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public float velocity = 10f;
    private Vector3 direction = new Vector3(1, 0, 0);

    // Use this for initialization
    void Start()
    {
        direction.Normalize();
    }
	
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = direction * velocity;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.GetComponent<Destructible>())
        {
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.GetComponent<Shot>() == null)
            Destroy(gameObject);
    }
}
