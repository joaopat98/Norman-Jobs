using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : BulletHit
{
    public float MaxRangeTime;

    void Update()
    {
        Destroy(gameObject, MaxRangeTime);
    }

}
