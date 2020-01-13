using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss2 : Boss
{
    private float t;
    [Header("Boss Settings")]
    public float timeBetweenActions = 1.0f;
    public float SpecialProbability = 0.2f;

    public float NoiseScale = 1;
    private float moveSeed;

    private bool wasWalking = false;
    private Vector2 walkDirection;
    public float MoveFrequency = 0.5f;

    public GameObject SlashPrefab;
    public float SlashSpeed;
    public float SlashDamage;
    public AudioClip SlashSound;
    public float SlashSoundVolume = 1;
    public float SeekDuration;
    public float SeekDamage;

    public float SeekRange;
    public float SeekRadius;
    public GameObject PlayerTriggerPrefab;

    public GameObject PistolPrefab, BatPrefab;
    public GameObject rangedEnemy;
    public GameObject meleeEnemy;

    public GameObject ExplosionPrefab;
    public float SelfKnockBack = 0.5f;

    private bool twisting;
    public float TwistSpeed;
    public float TwistDuration;
    private Vector2 lastDirection;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (twisting)
        {
            rb.velocity = Vector2.Reflect(lastDirection, col.contacts[0].normal);
            lastDirection = rb.velocity;
        }
    }

    new void Start()
    {
        base.Start();
        moveSeed = System.DateTime.Now.Millisecond;
    }
    protected override void Act()
    {
        wasWalking = false;
        acting = true;
        animator.SetTrigger("action");
        if (Random.Range(0f, 1f) < SpecialProbability)
        {
            animator.SetTrigger("twist");
        }
        else
        {
            var x = player.transform.position.x - transform.position.x;
            switch (Random.Range(0, 3))
            {
                case 0:

                    LookToSide(x);
                    animator.SetTrigger("seek");
                    break;
                case 1:
                    LookToSide(x);
                    animator.SetTrigger("slash");
                    break;
                case 2:
                    animator.SetTrigger("generate");
                    break;
            }
        }
    }

    protected override void Idle()
    {
        if (Mathf.PerlinNoise(Time.time * NoiseScale, moveSeed) < MoveFrequency)
        {
            if (!wasWalking)
            {
                float angle = Random.Range(0, Mathf.PI * 2);
                walkDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                float x = walkDirection.x;
                LookToSide(x);
            }
            wasWalking = true;
            animator.SetBool("moving", true);
            rb.MovePosition(rb.position + walkDirection * speed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("moving", false);
            wasWalking = false;
        }
    }

    protected new void FixedUpdate()
    {
        Vector2 S = spr.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        if (isAwake && !hurting && isAlive())
        {
            if (!acting)
            {
                if (t > timeBetweenActions)
                {
                    t = 0;
                    Act();
                }
                else
                {
                    Idle();
                }
            }
        }
        t += Time.fixedDeltaTime;
    }

    public void ThrowSlash()
    {
        var direction = (player.transform.position - transform.position).normalized;
        LookToSide(direction.x);
        GameObject shot = Instantiate(SlashPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<IDamaging>().setDamage(SlashDamage);
        shot.GetComponent<Rigidbody2D>().velocity = SlashSpeed * direction;
        shot.GetComponent<Rigidbody2D>().SetRotation(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
        AudioSource.PlayClipAtPoint(SlashSound, Camera.main.transform.position, SlashSoundVolume);
    }

    public void Seek()
    {
        StartCoroutine(SeekRoutine());
    }

    IEnumerator SeekRoutine()
    {
        var dir = (player.transform.position - transform.position);
        LookToSide(dir.x);
        var startPos = transform.position;
        Vector2 endPos;
        if (dir.magnitude > SeekRange)
            endPos = transform.position + dir.normalized * SeekRange;
        else
            endPos = player.transform.position;
        float t = 0;
        while (t < SeekDuration)
        {
            rb.MovePosition(Vector2.Lerp(startPos, endPos, t / SeekDuration));
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
    }

    public void SeekHit()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy").Select(e => e.GetComponent<Enemy>()))
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < SeekRadius)
            {
                enemy.Hit(gameObject, 1, 2);
            }
        }
        if (Vector3.Distance(transform.position, player.transform.position) < SeekRadius)
        {
            player.GetComponent<IHealthSystem>().Hit(gameObject, SeekDamage, 2);
        }
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }

    override public void Hit(GameObject obj, float value, float knockback)
    {
        if (acting)
        {
            base.Hit(obj, value, 0);
        }
        else
        {
            base.Hit(obj, value, SelfKnockBack);
        }
    }

    public void SpawnEnemy()
    {
        bool dropsWeapon = Random.Range(0f, 1f) > 0.66;
        if (Random.Range(0f, 1f) > 0.5)
        {
            var enemy = Instantiate(meleeEnemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
            if (dropsWeapon)
            {
                enemy.WeaponDrop = BatPrefab;
            }
            else
                enemy.WeaponDrop = null;

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
        }
    }

    public void Twist()
    {
        StartCoroutine(TwistRoutine());
    }

    IEnumerator TwistRoutine()
    {
        var playerTrigger = Instantiate(PlayerTriggerPrefab, rb.position, Quaternion.identity, transform).GetComponent<PlayerTrigger>();
        playerTrigger.Damage = SeekDamage;
        twisting = true;
        animator.SetBool("twisting", true);
        Vector2 dir = (player.transform.position - transform.position).normalized;
        twisting = true;
        rb.velocity = dir * TwistSpeed;
        lastDirection = rb.velocity;
        yield return new WaitForSeconds(TwistDuration);
        twisting = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("twisting", false);
        Destroy(playerTrigger.gameObject);
    }
}
