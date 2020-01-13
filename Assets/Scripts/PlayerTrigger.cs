using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public float Damage;
    private bool hasHit = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasHit && col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<IHealthSystem>().Hit(gameObject, Damage);
            hasHit = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
