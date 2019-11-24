using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public string[] targets;
    public string[] avoid;

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var target in targets)
        {
            if (col.CompareTag(target))
            {
                col.GetComponent<HealthSystem>().Hit(null, 1);
                Destroy(gameObject);
                return;
            }
        }
        foreach (var avoidable in avoid)
        {
            if (col.CompareTag(avoidable))
            {
                return;
            }
        }
        Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
