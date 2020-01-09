using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float retreatDistance;
    public float BulletSpeed = 0.001f;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject PistolPrefab, BatPrefab;


    public GameObject Bullet;
   

    public BossConfigs bossConfig;
    List<Transform> waypoints;
    private int waypointIndex = 0;

    private bool shooting;
    private bool melee;
    private bool stop;
    private GameObject rangedEnemy;
    private GameObject meleeEnemy;
    private bool awake;

    private void OnAnimatorMove()
    {
        rb.velocity = animator.deltaPosition / Time.deltaTime;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        stop = true;
        waypoints = bossConfig.getWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
        rangedEnemy = bossConfig.enemyRangedPrefab;
        meleeEnemy = bossConfig.enemyMeleePrefab;

    }


    new void FixedUpdate()
    {
        if (awake)
        {
            Move();
            if (!hurting)
                timeBtwShots -= Time.deltaTime;
        }
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

                rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
                animator.SetInteger("x", dir_ceil.x);
                animator.SetInteger("y", dir_ceil.y);

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
       
        if (player.GetComponent<PlayerHealth>().isAlive())
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletHitZigZag>().setDamage(Damage);
            dir = dir.normalized;
            bullet.GetComponent<BulletHitZigZag>().dir = new Vector3(dir.y, -dir.x);
                Debug.Log(dir);
            bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * dir;
            //bullet.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
            AudioSource.PlayClipAtPoint(attackSound, this.transform.position, attackSoundVolume);
        }


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
        bool enter = true;
        if (waypointIndex <= waypoints.Count - 1)
        {

            if (stop)
            {
                var targetPosition = waypoints[waypointIndex].transform.position;
                var movementThisFrame = bossConfig.getMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                Act();
                if (transform.position == targetPosition)
                {
                    Random random = new Random();
                    int shotType = Random.Range(0, 2);
                    if(shotType == 1 && enter)
                    {
                        enter = false;
                        StartCoroutine(SpawnEnemies());
                        enter = true;
                    }
                   
                    waypointIndex++;
                }
            }

        }
        else
        {
            waypointIndex = 0;
        }
    }

    IEnumerator SpawnEnemies()
    {
        stop = false;
        yield return new WaitForSeconds(2.0f);
        bool dropsWeapon = Random.Range(0f, 1f) > 0.66;
        if (melee)
        {
            var enemy = Instantiate(meleeEnemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
            if (dropsWeapon)
            {
                enemy.WeaponDrop = BatPrefab;
            }
            else
                enemy.WeaponDrop = null;
            melee = false;

        }
        else
        {
            var enemy = Instantiate(rangedEnemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
            if (dropsWeapon)
            {
                enemy.WeaponDrop = PistolPrefab;
            }
            else
                enemy.WeaponDrop = null;
            melee = true;

        }
        stop = true;


    }


    public void WakeUp()
    {
        awake = true;
    }

}
