﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Vector2Int lookingAt;
    private Rigidbody2D rb;
    private Animator animator;
    private Punch punch;
    private PlayerHealth health;

    private void OnAnimatorMove()
    {
        rb.velocity = animator.deltaPosition / Time.deltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        punch = GetComponent<Punch>();
        health = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!punch.punching && !health.hurting)
        {
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            lookingAt = dir.magnitude == 0 ? lookingAt : dir.ToSpriteDirection(0.2f);
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
            animator.SetInteger("x", lookingAt.x);
            animator.SetInteger("y", lookingAt.y);

            if (lookingAt.x > 0)
            {
                var scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
            else
            {
                var scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
            }
        }
    }
}
