using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public Punch punch;
    private bool hasHit = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasHit && col.CompareTag("Enemy"))
        {
            punch.HitEnemy(col.GetComponent<Enemy>());
            hasHit = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
