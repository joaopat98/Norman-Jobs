using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject Bullet;
    public float BulletSpeed = 1;
    public float ShootInterval;
    protected float shootTimer = 0;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        type = WeaponType.Ranged;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
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

    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            if (Ammo > 0)
            {
                GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
                shot.GetComponent<IDamaging>().setDamage(Damage);
                shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;
                shot.GetComponent<Rigidbody2D>().SetRotation(Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 135.0f);
                shootTimer = ShootInterval;
                AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);

                Ammo -= 1;

                if (Ammo == 0)
                {
                    mouse.SetWeapon(null);
                   
                    Destroy(gameObject);
                    Die();
                }
            }
        }
    }


}
