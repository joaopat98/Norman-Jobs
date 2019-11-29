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

    public GameObject Bullet;
    public float BulletSpeed = 1;
    public float ShootInterval;
    public float Damage;

    public int Ammo;

    private float shootTimer = 0;
    private GameObject thePlayer;
    private MouseMovement mouse;
    private SpriteRenderer spr;
    void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        mouse = thePlayer.GetComponent<MouseMovement>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            if (Ammo > 0)
            {
                GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
                shot.GetComponent<IDamaging>().setDamage(Damage);
                shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;
                shot.transform.Rotate(0, Mathf.Atan2(direction.y, direction.x), 0);
                shootTimer = ShootInterval;

                Ammo -= 1;

                if (Ammo == 0)
                {
                    mouse.SetWeapon(null);
                    Destroy(gameObject);
                }
            }
        }
    }
}