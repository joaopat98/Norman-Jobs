using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : BulletHit
{
    public float MaxRangeTime;
    private float t;

    private Renderer render;
    private Color color;

    void Start()
    {
        t = 0.0f;
        render = GetComponent<Renderer>();
        color = render.material.color;
    }

    void Update()
    {
        t += Time.deltaTime;

        var newColor = color;

        newColor.a = newColor.a * EasingFunction.EaseInCubic(1, 0, t / MaxRangeTime);

        render.material.color = newColor;

        Destroy(gameObject, MaxRangeTime);
    }

}
