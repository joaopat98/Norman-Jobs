using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float retreatDistance;
    public float BulletSpeed = 1;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject Bullet;

    BossConfigs bossConfig;
    List<Transform> waypoints;
    private int waypointIndex = 0;

    private bool shooting;


    private void OnAnimatorMove()
    {
        rb.velocity = animator.deltaPosition / Time.deltaTime;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timeBtwShots = startTimeBtwShots;

        waypoints = bossConfig.getWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (!hurting)
            timeBtwShots -= Time.deltaTime;
    }

    protected override void Act()
    {
        if (!hurting)
        {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            Vector2 dir = (player.transform.position - transform.position).normalized;
            Vector2Int dir_ceil = dir.ToSpriteDirection(0.2f);

            if (dir.x > 0)
            {
                var scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
            else
            {
                var scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

            if (!shooting)
            {
                if (dist > stopDistance)
                {
                    rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
                    animator.SetInteger("x", dir_ceil.x);
                    animator.SetInteger("y", dir_ceil.y);
                }
                else if (dist < retreatDistance)
                {
                    rb.MovePosition(rb.position - (dir * speed * Time.fixedDeltaTime));
                    animator.SetInteger("x", -dir_ceil.x);
                    animator.SetInteger("y", -dir_ceil.y);
                }
            }

            if (timeBtwShots <= 0 && dist < stopDistance)
            {
                shooting = true;
                animator.SetInteger("x", dir_ceil.x);
                animator.SetInteger("y", dir_ceil.y);
                animator.SetTrigger("shoot");
                timeBtwShots = startTimeBtwShots;
            }
        }
    }

    public void Shoot()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletHit>().setDamage(Damage);
        bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * dir;
        bullet.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);

    }

    public void FinishShooting()
    {
        shooting = false;
    }

    protected override void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = bossConfig.getMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
