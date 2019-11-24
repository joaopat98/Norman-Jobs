using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, HealthSystem
{

    public float speed;
    public float stopDistance;
    protected GameObject player;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hit(GameObject obj, int value)
    {
        Destroy(gameObject);
    }
}



