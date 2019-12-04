using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{

    public float timeBtwAttacks;
    public float startTimeBtwAttacks;
    public float attackDistance;
    public float BoxSize;
    // public int Ammo;
    // public int Damage;
    // public GameObject Slash;

    private Animator anim;
    private PlayerMovement movement;
    private GameObject player;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector2 dir = movement.lookingAt;

        if (timeBtwAttacks <= 0 && Input.GetButtonDown("Fire1") && player.GetComponent<MouseMovement>().GetWeapon() != null
            && player.GetComponent<MouseMovement>().GetWeapon() == this)
        {

            anim.SetTrigger("bat");
            anim.SetBool("go_Normal", false);
            timeBtwAttacks = startTimeBtwAttacks;

            /*var slash = Instantiate(Slash, transform.position, Quaternion.identity);
            slash.GetComponent<IDamaging>().setDamage(Damage);*/

        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }

    public void TryAttack()
    {
        Vector2 dir = movement.lookingAt;

        var hit = Physics2D.BoxCast(player.transform.position, Vector2.one * BoxSize, 0, dir, attackDistance, LayerMask.GetMask("Enemies"));
        if (hit.collider != null)
        {

            hit.transform.GetComponent<Enemy>().Hit(null, Damage);
            Ammo--;

            if (Ammo == 0)
            {

                Destroy(gameObject);
            }
        }
    }

    public void FinishAttack()
    {

        anim.SetBool("go_Normal", true);
    }


}
