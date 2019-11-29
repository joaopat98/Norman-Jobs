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
    
    private Rigidbody2D rb;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timeBtwShots = startTimeBtwShots;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        timeBtwShots -= Time.deltaTime;
    }

    protected override void Act()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > stopDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.velocity = speed * dir;

        }
        else if (dist < stopDistance && dist > retreatDistance)
        {
            rb.velocity = Vector3.zero;
        }
        else if (dist < retreatDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.velocity = speed * -dir;
        }

        if (timeBtwShots <= 0)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletHit>().setDamage(1);
            bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * dir;
            bullet.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
            timeBtwShots = startTimeBtwShots;
        }
    }

    protected override void Idle()
    {
        rb.velocity = Vector3.zero;
    }
}
