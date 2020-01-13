using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeExplosion : MonoBehaviour
{
    public float damage = 3.0f;
    public float KnockBack = 2.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHealthSystem>().Hit(gameObject, damage, KnockBack);
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
