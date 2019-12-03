using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour, IDamaging
{
    public string[] targets;
    public string[] avoid;
    public GameObject weapon;
    private float damage;
    public bool contact;

    void start()
    {
        contact = false;
    }

    public float GetDamage()
    {
        return damage;
    }

    public IEnumerator DestroySlash()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public void setDamage(float value)
    {
        damage = value;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var target in targets)
        {
         
            if (col.CompareTag(target))
            {
                contact = true;
                col.GetComponent<IHealthSystem>().Hit(null, GetDamage());
                StartCoroutine(DestroySlash());
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
        StartCoroutine(DestroySlash());
    }

    void Update()
    {
        StartCoroutine(DestroySlash());
    }
}
