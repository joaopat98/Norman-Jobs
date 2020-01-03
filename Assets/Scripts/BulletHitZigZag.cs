using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitZigZag : BulletHit
{

    private Vector3 axis;
    public Vector3 dir;

    private Rigidbody2D rb;
    //Speed, size
    private float frequency, magnitude;


    // Start is called before the first frame update
    void Start()
    {

        frequency = 20.0f;
        magnitude = 0.5f;
        axis  = transform.up;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

         transform.position += dir * Mathf.Sin(Time.time * frequency) * magnitude;
            


    }
}
