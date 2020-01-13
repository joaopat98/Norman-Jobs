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
    protected Color oldColor;
    protected bool hurting;


    public float attackSoundVolume;
    public AudioClip attackSound;
    public float deathSoundVolume = 0.1f;
    public AudioClip deathSound;

    public bool SpriteFlipped = false;

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
        Vector2 S = spr.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        if (!hurting && player.GetComponent<IHealthSystem>().isAlive() && isAlive())
        {
            float dist = Vector2.Distance(player.transform.position, transform.position);
            if (dist < Radar)
                Act();
            else
            {
                Idle();
            }
        }
    }
    public virtual void Hit(GameObject obj, float value)
    {
        Hit(obj, value, value);
    }

    public virtual void Hit(GameObject obj, float value, float knockback)
    {
        Radar = Mathf.Infinity;
        HP -= value;

        if (HP <= 0)
        {
            player.GetComponent<ScoreScriptPlayer>().FinalDamage();
        }

        else
        {
            player.GetComponent<ScoreScriptPlayer>().Damage();
        }

        StartCoroutine(Tint());
        if (HP <= 0)
        {
            if (WeaponDrop)
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            GetComponent<Collider2D>().enabled = false;
            animator.SetBool("alive", false);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
            return;
        }
        else if (!hurting)
        {
            hurting = true;
            var dir = ((Vector2)(obj.transform.position - transform.position)).ToSpriteDirection(0.2f);
            animator.SetInteger("x", Mathf.RoundToInt(dir.x));
            animator.SetInteger("y", Mathf.RoundToInt(dir.y));
            StartCoroutine(PushBack(transform.position - obj.transform.position, knockback));
        }
        animator.SetBool("hurt", true);

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

    public void Die()
    {
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        int temp = 0;
        var flashingColor = new Color(1, 1, 1, 0);
        var regularColor = new Color(1, 1, 1, 1);
        while (temp < 3)
        {
            spr.color = flashingColor;
            yield return new WaitForSeconds(0.1f);
            spr.color = regularColor;
            yield return new WaitForSeconds(0.1f);
            temp++;
        }
        Destroy(gameObject);
    }

    public bool isAlive()
    {
        return HP > 0;
    }
}



