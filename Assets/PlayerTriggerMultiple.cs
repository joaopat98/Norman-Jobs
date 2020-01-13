using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerMultiple : MonoBehaviour
{
    // Start is called before the first frame update
    public float Damage;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<IHealthSystem>().Hit(gameObject, Damage);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
