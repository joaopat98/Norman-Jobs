using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float retreatDistance;
    public float stopDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.position, transform.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(player.position, transform.position) < stopDistance && Vector2.Distance(player.position, transform.position) >retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(player.position, transform.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        
        if(timeBtwShots <= 0)
        {
            Instantiate(projectile, this.transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
