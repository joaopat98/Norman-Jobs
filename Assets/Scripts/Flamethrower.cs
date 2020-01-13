using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : RangedWeapon
{
    private bool justOne = true;
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

                if (justOne)
                {
                    StartCoroutine(FireSound());
                }

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

    IEnumerator FireSound()
    {
        justOne = false;
        AudioSource.PlayClipAtPoint(weaponSound, Camera.main.transform.position, weaponSoundVolume);
        yield return new WaitForSeconds(0.28888f);
        justOne = true;
    }
}
