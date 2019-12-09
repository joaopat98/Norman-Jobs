using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        mouse = GetComponent<MouseMovement>();
    }

    void Update()
    {
        if (health.isAlive() && !health.hurting)
        {
            if (timeBtwPunches <= 0 && Input.GetButtonDown("Fire2"))
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
                timeBtwPunches = startTimeBtwPunches;
            }
            else
            {
                timeBtwPunches -= Time.deltaTime;
            }
        }
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
}
