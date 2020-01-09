using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenHit : BulletHit
{
    public float RotateSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * RotateSpeed * Time.deltaTime, Space.World);
    }
}
