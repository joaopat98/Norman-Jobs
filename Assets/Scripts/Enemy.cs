using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHealthSystem
{

    public float speed;
    public float stopDistance;
    protected GameObject player;

    public GameObject WeaponDrop;

    public float Radar;

    public float HP;

    public float Damage = 1f;

    public float HitTime = 0.25f, HitDistance = 1f;

    protected SpriteRenderer spr;
    protected Animator animator;
    protected Rigidbody2D rb;
    private Color oldColor;

    protected bool hurting;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        oldColor = spr.color;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist < Radar)
            Act();
        else
        {
            Idle();
        }
    }
    public virtual void Hit(GameObject obj, float value)
    {
        HP -= value;
        StartCoroutine(Tint());
        if (HP <= 0)
        {
            if (WeaponDrop)
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            if (!hurting)
            {
                hurting = true;
                StartCoroutine(PushBack(transform.position - obj.transform.position, value));
            }
            animator.SetTrigger("hurt");
        }

    }

    protected IEnumerator Tint()
    {
        spr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spr.color = oldColor;
    }

    protected abstract void Act();
    protected abstract void Idle();


    public void FinishHurting()
    {
        hurting = false;
    }

    IEnumerator PushBack(Vector3 direction, float damage)
    {
        float curTime = 0;
        Vector2 finalPos = transform.position + (direction.normalized * HitDistance * damage);
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



