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

    public float HitDistance = 1;
    public float HitTime = 0.25f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool shooting;

    private bool hurting;

    private void OnAnimatorMove()
    {
        rb.velocity = animator.deltaPosition / Time.deltaTime;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timeBtwShots = startTimeBtwShots;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            Vector2Int dir_ceil = new Vector2Int();
            if (Mathf.Abs(dir.x) > 0.2f)
                dir_ceil.x = dir.x > 0 ? 1 : -1;
            else
                dir_ceil.x = 0;
            if (Mathf.Abs(dir.y) > 0.2f)
                dir_ceil.y = dir.y > 0 ? 1 : -1;
            else
                dir_ceil.y = 0;

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
        bullet.GetComponent<BulletHit>().setDamage(1);
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

    public override void Hit(GameObject obj, float value)
    {
        HP -= value;
        //StartCoroutine(Tint());
        if (HP <= 0)
        {
            if (WeaponDrop)
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            hurting = true;
            StartCoroutine(PushBack(transform.position - obj.transform.position));
            animator.SetTrigger("hurt");
        }
    }

    public void FinishHurting()
    {
        hurting = false;
    }

    IEnumerator PushBack(Vector3 direction)
    {
        float curTime = 0;
        Vector2 finalPos = transform.position + (direction.normalized * HitDistance);
        Vector2 origPos = transform.position;
        float startTime = Time.time;
        while (curTime < HitTime)
        {
            rb.MovePosition(Vector2.Lerp(origPos, finalPos, EasingFunction.EaseOutCubic(0, 1, curTime / HitTime)));
            yield return new WaitForFixedUpdate();
            curTime = Time.time - startTime;
        }
    }
}
