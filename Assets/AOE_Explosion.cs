using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Explosion : MonoBehaviour
{
    public float MaxRadius = 1;
    public float ExplosionTime = 1;
    private float t = 0;
    private SpriteRenderer spr;
    private Color baseColor;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        baseColor = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (t / ExplosionTime < 1)
        {
            var newColor = baseColor;
            newColor.a *= 1 - EasingFunction.EaseOutCubic(0, 1, t / ExplosionTime);
            spr.color = newColor;
            transform.localScale = Vector3.one * EasingFunction.EaseOutCubic(0, 1, t / ExplosionTime) * MaxRadius;
        }
        else
        {
            Destroy(gameObject);
        }
        t += Time.deltaTime;
    }
}
