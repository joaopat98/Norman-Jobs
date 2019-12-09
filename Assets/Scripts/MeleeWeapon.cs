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
    public float AttackTime;

    private bool attacking;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (attacking && col.CompareTag("Enemy"))
        {
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
    }

    void Update()
    {
        if (timeBtwAttacks <= 0 && Input.GetButtonDown("Fire1") && thePlayer.GetComponent<MouseMovement>().GetWeapon() != null
       && thePlayer.GetComponent<MouseMovement>().GetWeapon() == this)
        {
            timeBtwAttacks = startTimeBtwAttacks;
            Ammo -= 1;
            attacking = true;
            StartCoroutine(Swipe());
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }

    IEnumerator Swipe()
    {
        Vector2 dir = (mouse.MousePos - (Vector2)thePlayer.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        float startAngle = angle - Mathf.Deg2Rad * angleDelta / 2;
        float endAngle = angle + Mathf.Deg2Rad * angleDelta / 2;
        float t = 0;
        AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);
        while (t < AttackTime)
        {
            float curAngle = MathfMap.Map(t, 0, AttackTime, startAngle, endAngle);

            if (thePlayer.transform.localScale.x > 0)
                transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * curAngle - 45);
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * curAngle - 135);
            transform.position = thePlayer.transform.position + new Vector3(Mathf.Cos(curAngle), Mathf.Sin(curAngle)) * AttackRadius;
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        attacking = false;
        if (Ammo == 0)
        {
            mouse.SetWeapon(null);
            Destroy(gameObject);
        }
    }
}
