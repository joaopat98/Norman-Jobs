using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, HealthSystem
{
    public float speed;
    public float retreatDistance;
    public float stopDistance;
    public float BulletSpeed = 1;
    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject Bullet;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

    public void Hit(GameObject obj, int value)
    {
        Destroy(gameObject);
    }
}
