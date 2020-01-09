using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagWeapon : RangedWeapon
{
    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            if (Ammo > 0)
            {
                direction = direction.normalized;
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                bullet.GetComponent<BulletHitZigZag>().setDamage(Damage);
               
                bullet.GetComponent<BulletHitZigZag>().dir = new Vector3(direction.y, -direction.x);
                
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;


                bullet.transform.Rotate(0, Mathf.Atan2(direction.y, direction.x), 0);
                shootTimer = ShootInterval;
                AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);

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
