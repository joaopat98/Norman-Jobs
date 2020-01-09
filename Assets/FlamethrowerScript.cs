using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerScript : RangedWeapon
{
    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            if (Ammo > 0)
            {
                GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
                shot.GetComponent<IDamaging>().setDamage(Damage);
                shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;

                shot.GetComponent<Rigidbody2D>().SetRotation(Mathf.Atan2(direction.y, direction.x));

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
