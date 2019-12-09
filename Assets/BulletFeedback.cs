using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFeedback : MonoBehaviour
{
    // Start is called before the first frame update
    public float Duration = 0.5f;
    float t;
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (t >= Duration)
        {
            Destroy(gameObject);
        }
        t += Time.deltaTime;
    }
}
