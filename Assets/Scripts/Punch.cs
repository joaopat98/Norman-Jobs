using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Punch : MonoBehaviour
{

    public float timeBtwPunches;
    public float startTimeBtwPunches;
    public float AttackRange;
    public float Damage;
    public float BoxSize;
    public bool punching = false;
    public float AttackTime = 0.25f, AttackDistance = 1f;
    public AudioClip punchSound;
    public AudioClip tryPunchSound;
    public float punchSoundVolume;
    public GameObject EnemyTriggerPrefab;
    private PlayerHealth health;
    private Animator anim;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    private MouseMovement mouse;
    private GameObject enemyTrigger;

    public GameObject player;

    private Transform chargeRing;

    private Vector3 origChargeScale;

    public float InitChargeVel = 1;
    public float FinalChargeVel = 10;
    public float ChargeTime = 3;
    public float TimeBeforeCharging = 0.5f;

    public Color ChargingColor, ChargedColor;
    private bool charging;
    private bool charged;

    public float PunchAOERadius = 5;
    public float PunchAOEDamage = 0.5f;

    public GameObject ExplosionPrefab;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        mouse = GetComponent<MouseMovement>();
        chargeRing = transform.Find("charge_ring");
        chargeRing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        origChargeScale = chargeRing.localScale;
    }

    void Update()
    {
        if (health.isAlive() && !health.hurting)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(ChargePunch());
            }
            if (timeBtwPunches <= 0 && Input.GetButtonUp("Fire2"))
            {
                charging = false;
                if (!charged)
                {
                    punching = true;
                    anim.SetTrigger("punch");
                    Vector2 dir = (mouse.MousePos - rb.position).normalized;
                    var dir_sprite = dir.ToSpriteDirection(0.2f);
                    anim.SetInteger("x", dir_sprite.x);
                    anim.SetInteger("y", dir_sprite.y);
                    if (dir_sprite.x > 0)
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
                    StartCoroutine(MoveTo(mouse.MousePos - rb.position));
                }
                timeBtwPunches = startTimeBtwPunches;
            }
            else
            {
                timeBtwPunches -= Time.deltaTime;
            }
        }
        if (health.isAlive() && Input.GetButtonUp("Fire2"))
            charged = false;
        Debug.DrawLine(transform.position, transform.position + Vector3.up * PunchAOERadius, Color.green, Time.fixedDeltaTime);
    }

    public void TryPunch()
    {
        GetComponent<PlayerHealth>().hurting = false;
        AudioSource.PlayClipAtPoint(tryPunchSound, Camera.main.transform.position, punchSoundVolume);
        enemyTrigger = Instantiate(EnemyTriggerPrefab, rb.position, Quaternion.identity, transform);
        enemyTrigger.GetComponent<EnemyTrigger>().punch = this;
    }

    public void HitEnemy(Enemy enemy)
    {
        enemy.Hit(gameObject, Damage);
        AudioSource.PlayClipAtPoint(punchSound, Camera.main.transform.position, punchSoundVolume);
    }

    public void FinishPunch()
    {
        punching = false;
        health.hurting = false;
        Destroy(enemyTrigger);
    }

    IEnumerator MoveTo(Vector2 direction)
    {
        float curTime = 0;

        Vector2 finalPos;
        if (direction.magnitude > AttackDistance)
        {
            finalPos = (Vector2)transform.position + (direction.normalized * AttackDistance);
        }
        else
        {
            finalPos = (Vector2)transform.position + direction;
        }
        Vector2 origPos = transform.position;
        float startTime = Time.time;
        while (curTime < AttackTime)
        {
            rb.MovePosition(Vector2.Lerp(origPos, finalPos, EasingFunction.EaseOutCubic(0, 1, curTime / AttackTime)));
            yield return new WaitForFixedUpdate();
            curTime = Time.time - startTime;
        }
    }

    public void PunchAOE()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy").Select(e => e.GetComponent<Enemy>()))
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < PunchAOERadius)
            {
                enemy.Hit(gameObject, PunchAOEDamage, 2);
            }
        }
    }

    IEnumerator ChargePunch()
    {
        charging = true;
        float chargeVel = InitChargeVel;
        float t = 0;
        float curSize = 1;
        var spr = chargeRing.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(TimeBeforeCharging);
        spr.color = ChargingColor;
        while (t < ChargeTime && charging)
        {
            curSize -= chargeVel * Time.fixedDeltaTime;
            if (curSize <= 0)
            {
                curSize = 1;
            }
            chargeVel = Mathf.Lerp(InitChargeVel, FinalChargeVel, t / ChargeTime);
            chargeRing.localScale = origChargeScale * EasingFunction.EaseOutQuad(0, 1, curSize);
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
        spr.color = ChargedColor;
        if (charging)
        {
            charged = true;
            while (charged)
            {
                curSize -= chargeVel * Time.fixedDeltaTime;
                if (curSize <= 0)
                {
                    curSize = 1;
                }
                chargeRing.localScale = origChargeScale * EasingFunction.EaseOutQuad(0, 1, curSize);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitWhile(() => health.hurting);
            anim.SetTrigger("punchAOE");
            anim.SetTrigger("punch");
        }
        spr.color = new Color(0, 0, 0, 0);
    }
}