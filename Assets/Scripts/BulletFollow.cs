using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : BulletHit, IDamaging
{
    private Rigidbody2D rb;
    private float velocity;
    private Transform player;

    public float follow = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = GetComponent<Rigidbody2D>().velocity.magnitude;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.Lerp(rb.velocity.normalized, (player.position - transform.position).normalized, follow * Time.deltaTime).normalized * velocity;
        rb.SetRotation(Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x));
    }
}
