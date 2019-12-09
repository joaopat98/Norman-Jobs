using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour, IDamaging
{
    public GameObject FeedbackPrefab;
    public string[] targets;
    public string[] avoid;

    private float damage;

    public float GetDamage()
    {
        return damage;
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

                col.GetComponent<IHealthSystem>().Hit(gameObject, GetDamage());
                Instantiate(FeedbackPrefab, transform.position, Quaternion.identity);
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
