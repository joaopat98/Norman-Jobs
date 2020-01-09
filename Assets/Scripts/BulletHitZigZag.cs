using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitZigZag : BulletHit
{

    private Vector3 axis;
    public Vector3 dir;

    private Rigidbody2D rb;
    //Speed, size
    public float frequency, magnitude;


    // Start is called before the first frame update
    void Start()
    {

       
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

         transform.position += dir * Mathf.Sin(Time.time * frequency) * magnitude;
            


    }
}
