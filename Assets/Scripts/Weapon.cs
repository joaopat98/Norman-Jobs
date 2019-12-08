using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

public class Weapon : MonoBehaviour
{


    // Start is called before the first frame update
    public WeaponType type;

    public AudioClip weaponSound;
    public float weaponSoundVolume;

    /* public GameObject Bullet;
     public float BulletSpeed = 1;
     public float ShootInterval;*/
    public float Damage;

    public int Ammo;

   // private float shootTimer = 0;
    protected GameObject thePlayer;
    protected MouseMovement mouse;
    protected SpriteRenderer spr;


    protected void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        mouse = thePlayer.GetComponent<MouseMovement>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
   /* void FixedUpdate()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.fixedDeltaTime;
        }
    }

    void LateUpdate()
    {
        if (thePlayer != null)
        {
            if (mouse.GetWeapon() == this)
                spr.sortingOrder = thePlayer.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }*/

    public virtual void Shoot(Vector3 origin, Vector3 direction)
    {

    }
}