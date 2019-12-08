using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    public float timeBtwAttacks;
    public float startTimeBtwAttacks;
    public float attackDistance;
    public float BoxSize = 1.0f;

    private Animator anim;
    private PlayerMovement movement;
    public AudioClip hitSound;


    new void Start()
    {
       base.Start();
       anim = GetComponent<Animator>();
       type = WeaponType.Melee;
       movement = thePlayer.GetComponent<PlayerMovement>();
     
    }

    void Update()
    {
        Vector2 dir = movement.lookingAt;
        Debug.DrawRay(transform.position, dir * attackDistance, Color.green, 0.016f);
        if (timeBtwAttacks <= 0 && Input.GetButtonDown("Fire1") && thePlayer.GetComponent<MouseMovement>().GetWeapon() != null
            && thePlayer.GetComponent<MouseMovement>().GetWeapon() == this)
        {

            anim.SetTrigger("bat");
            anim.SetBool("go_Normal", false);


            timeBtwAttacks = startTimeBtwAttacks;

        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }

    public void TryAttack()
    {
        Vector2 dir = movement.lookingAt;

        var hit = Physics2D.BoxCast(thePlayer.transform.position, Vector2.one * BoxSize, 0, dir, attackDistance, LayerMask.GetMask("Enemies"));
        thePlayer.GetComponent<PlayerHealth>().hurting = false;
        
        if (hit.collider != null)
        {
           
            hit.transform.GetComponent<Enemy>().Hit(thePlayer, Damage);
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, weaponSoundVolume);

            Ammo -= 1;

            if (Ammo == 0)
            {
                mouse.SetWeapon(null);
                Destroy(gameObject);
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);
        }
       

    }

    public void FinishAttack()
    {
        anim.SetBool("go_Normal", true);
    }




}
