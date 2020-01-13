using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    public float timeBtwAttacks;
    public float startTimeBtwAttacks;
    public float AttackDistance;
    public float AttackRadius;
    public float BoxSize = 1.0f;
    public float angleDelta = 30f;

    private PlayerMovement movement;
    public AudioClip hitSound;


    private Rigidbody2D rb;
    public float AttackTime;

    private bool attacking;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (attacking && col.CompareTag("Enemy"))
        {
            Ammo -= 1;
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, weaponSoundVolume);
            col.GetComponent<IHealthSystem>().Hit(gameObject, 1);
        }
    }

    new void Start()
    {
        base.Start();
        attacking = false;
        type = WeaponType.Melee;
        movement = thePlayer.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    new void FixedUpdate()
    {
        if (!attacking)
            base.FixedUpdate();
        timeBtwAttacks -= Time.deltaTime;
    }

    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (timeBtwAttacks <= 0 && Input.GetButtonDown("Fire1") && Held)
        {
            timeBtwAttacks = startTimeBtwAttacks;
            
            attacking = true;
            StartCoroutine(Swipe(direction));
        }
    }

    IEnumerator Swipe(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x);
        float startAngle = angle - Mathf.Deg2Rad * angleDelta / 2;
        float endAngle = angle + Mathf.Deg2Rad * angleDelta / 2;
        float t = 0;
        AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);
        var oldRotation = rb.rotation;
        while (t < AttackTime)
        {
            float curAngle = MathfMap.Map(t, 0, AttackTime, startAngle, endAngle);
            rb.rotation = oldRotation + Mathf.Rad2Deg * curAngle - 45;
            rb.MovePosition(thePlayer.transform.position + new Vector3(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * AttackRadius);
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
        rb.rotation = oldRotation;
        rb.MovePosition(thePlayer.transform.position);
        attacking = false;
        if (Ammo == 0)
        {
            
            mouse.SetWeapon(null);
            Destroy(gameObject);
            Die();

        }
    }
}
