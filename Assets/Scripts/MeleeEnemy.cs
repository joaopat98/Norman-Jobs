﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    // Start is called before the first frame update
    private float timeBtwPunches;
    public float startTimeBtwPunches;
    public float punchDuration;
    public float attackDistance;

    public GameObject Punch;
    public Sprite attackSprite;
    public Sprite normalSprite;
    public SpriteRenderer sprite;

    private Rigidbody2D rb;
    new void Start()
    {
        base.Start();
        timeBtwPunches = startTimeBtwPunches;
        normalSprite = GetComponent<SpriteRenderer>().sprite;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        timeBtwPunches -= Time.deltaTime;
    }
    protected override void Act()
    {
        if (player != null && Vector2.Distance(player.transform.position, transform.position) > stopDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.velocity = speed * dir;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        if (player != null && Vector2.Distance(player.transform.position, transform.position) < attackDistance)
        {
            if (timeBtwPunches <= 0)
            {
                timeBtwPunches = startTimeBtwPunches;
                player.GetComponent<IHealthSystem>().Hit(gameObject, 2);
                StartCoroutine(ChangeSprite());

            }
        }
    }

    protected override void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    IEnumerator ChangeSprite()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        GameObject punch = Instantiate(Punch, player.transform.position, Quaternion.identity);

        punch.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
        sprite.sprite = attackSprite;
        yield return new WaitForSeconds(punchDuration);
        sprite.sprite = normalSprite;
        Destroy(punch);
    }
}
