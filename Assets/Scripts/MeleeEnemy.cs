using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    // Start is called before the first frame update
    private float timeBtwPunches;
    public float startTimeBtwPunches;
    public float punchDuration;
    public float AttackRange;

    public float AttackTime;
    public float AttackDistance;
    public float BoxSize = 1;

    private Vector2 punchDir;
    private bool punching;
    

    new void Start()
    {
        base.Start();
        timeBtwPunches = startTimeBtwPunches;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        timeBtwPunches -= Time.deltaTime;
    }

    protected override void Act()
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

        if (player != null && dist > stopDistance && !punching)
        {
            rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
            animator.SetInteger("x", dir_ceil.x);
            animator.SetInteger("y", dir_ceil.y);
        }
        if (player != null && dist < AttackRange && timeBtwPunches <= 0)
        {
            punching = true;
            timeBtwPunches = startTimeBtwPunches;
            animator.SetTrigger("punch");
            punchDir = (Vector2)player.transform.position - rb.position;
            MoveTo(punchDir);
        }
    }

    protected override void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    public void TryPunch()
    {
        var hit = Physics2D.BoxCast(transform.position, Vector2.one * BoxSize, 0, punchDir, AttackRange, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            hit.transform.GetComponent<IHealthSystem>().Hit(gameObject, Damage);
            AudioSource.PlayClipAtPoint(attackSound, this.transform.position, attackSoundVolume);
        }
    }

    public void FinishPunching()
    {
        punching = false;
    }

    IEnumerator MoveTo(Vector2 direction)
    {
        float curTime = 0;
       
        Vector2 finalPos = (Vector2)transform.position + (direction.normalized * AttackDistance);
        Vector2 origPos = transform.position;
        float startTime = Time.time;
        while (curTime < AttackTime)
        {
            rb.MovePosition(Vector2.Lerp(origPos, finalPos, EasingFunction.EaseOutCubic(0, 1, curTime / AttackTime)));
            yield return new WaitForFixedUpdate();
            curTime = Time.time - startTime;
        }
    }
}
