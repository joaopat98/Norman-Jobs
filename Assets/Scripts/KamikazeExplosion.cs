using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeExplosion : MonoBehaviour
{
    public float damage = 3.0f;
    private bool hasExploded = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHealthSystem>().Hit(gameObject, damage);
            
        }
        
        
    }

    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            StartCoroutine(RM());
        }
              
    }
    private IEnumerator RM()
    {
        hasExploded = true;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
