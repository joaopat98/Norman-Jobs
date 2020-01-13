using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss1 : Enemy
{
    private float t;
    [Header("Boss Settings")]
    public float timeBetweenActions = 1.0f;
    public float SpecialProbability = 0.2f;

    public float NoiseScale = 1;
    private float moveSeed;

    private bool wasWalking = false;
    private Vector2 walkDirection;

    private bool acting;
    public float MoveFrequency = 0.5f;

    public GameObject PunchPrefab;
    public float PunchSpeed;
    public float PunchDamage;
    public AudioClip PunchSound;
    public AudioClip ShockwaveSound;
    public AudioClip HurtSound;
    public AudioClip SlideSound;
    public AudioClip ClapSound;
    public float PunchSoundVolume = 1;
    public float SeekDuration;
    public float SeekDamage;

    public float SeekRange;
    public GameObject PlayerTriggerPrefab;

    public GameObject PistolPrefab, BatPrefab;
    public GameObject rangedEnemy;
    public GameObject meleeEnemy;

    public GameObject ExplosionPrefab;
    public float SelfKnockBack = 1;
    public float SpecialRadius;
    public float SpecialDamage;

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
            animator.SetTrigger("special");
        }
        else
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    var x = player.transform.position.x - transform.position.x;
                    if (SpriteFlipped ? x < 0 : x >= 0)
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
                    animator.SetTrigger("seek");
                    break;
                case 1:
                    animator.SetTrigger("throw");
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
                if (SpriteFlipped ? x < 0 : x >= 0)
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
        if (!hurting && isAlive())
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
        Debug.DrawRay(transform.position, Vector3.down * SpecialRadius, Color.green, Time.fixedDeltaTime);
    }

    public void FinishAction()
    {
        acting = false;
    }

    public void ThrowPunches()
    {
        var direction = (player.transform.position - transform.position).normalized;
        GameObject shot = Instantiate(PunchPrefab, transform.position, Quaternion.identity);
        shot.GetComponent<IDamaging>().setDamage(PunchDamage);
        shot.GetComponent<Rigidbody2D>().velocity = PunchSpeed * direction;
        shot.GetComponent<Rigidbody2D>().SetRotation(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
        AudioSource.PlayClipAtPoint(PunchSound, Camera.main.transform.position, PunchSoundVolume);
    }

    public void Seek()
    {
        StartCoroutine(SeekRoutine());
    }

    IEnumerator SeekRoutine()
    {
        var playerTrigger = Instantiate(PlayerTriggerPrefab, rb.position, Quaternion.identity, transform).GetComponent<PlayerTrigger>();
        playerTrigger.Damage = SeekDamage;
        AudioSource.PlayClipAtPoint(SlideSound, Camera.main.transform.position, PunchSoundVolume);
        var dir = (player.transform.position - transform.position).normalized;
        var startPos = transform.position;
        var endPos = transform.position + dir * SeekRange;
        float t = 0;
        while (t < SeekDuration)
        {
            rb.MovePosition(Vector2.Lerp(startPos, endPos, t / SeekDuration));
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
        Destroy(playerTrigger.gameObject);
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
        AudioSource.PlayClipAtPoint(HurtSound, Camera.main.transform.position, PunchSoundVolume);
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
        AudioSource.PlayClipAtPoint(ClapSound, Camera.main.transform.position, PunchSoundVolume);
    }

    public void Special()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy").Select(e => e.GetComponent<Enemy>()))
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < SpecialRadius)
            {
                enemy.Hit(gameObject, 1, 2);
            }
        }
        if (Vector3.Distance(transform.position, player.transform.position) < SpecialRadius)
        {
            player.GetComponent<IHealthSystem>().Hit(gameObject, SpecialDamage, 2);
        }
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(ShockwaveSound, Camera.main.transform.position, PunchSoundVolume);
    }
}
