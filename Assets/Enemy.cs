using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHealthSystem
{

    public float speed;
    public float stopDistance;
    protected GameObject player;

    public GameObject WeaponDrop;

    public float Radar;

    public float HP;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected void Update()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist < Radar)
            Act();
        else
        {
            Idle();
        }
    }
    public void Hit(GameObject obj, float value)
    {
        HP -= value;
        if (HP <= 0)
        {
            if (WeaponDrop)
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected abstract void Act();
    protected abstract void Idle();
}



