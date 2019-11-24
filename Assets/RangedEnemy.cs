using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public float retreatDistance;
    public float BulletSpeed = 1;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject Bullet;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > stopDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            this.GetComponent<Rigidbody2D>().velocity = speed * dir * Time.deltaTime;

        }
        else if (Vector2.Distance(player.transform.position, transform.position) < stopDistance && Vector2.Distance(player.transform.position, transform.position) > retreatDistance)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else if (Vector2.Distance(player.transform.position, transform.position) < retreatDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            this.GetComponent<Rigidbody2D>().velocity = speed * -dir * Time.deltaTime;
        }

        if (timeBtwShots <= 0)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * dir;
            bullet.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
