using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : RangedWeapon
{
    public override void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            if (Ammo > 0)
            {
                //Here we calculate the direction of the second shot
                float SecondShotXDirection = direction.x * Mathf.Cos(Mathf.PI / 10) + direction.y * Mathf.Sin(Mathf.PI / 10);
                float SecondShotYDirection = -direction.x * Mathf.Sin(Mathf.PI /10) + direction.y * Mathf.Cos(Mathf.PI / 10);
                Vector3 SecondShotDir = new Vector3(SecondShotXDirection, SecondShotYDirection, 0.0f);

                //Here we calculate the direction of the third shot
                float ThirdShotXDirection = direction.x * Mathf.Cos(Mathf.PI / 10) - direction.y * Mathf.Sin(Mathf.PI / 10);
                float ThirdShotYDirection = direction.x * Mathf.Sin(Mathf.PI / 10) + direction.y * Mathf.Cos(Mathf.PI / 10);
                Vector3 ThirdShotDir = new Vector3(ThirdShotXDirection, ThirdShotYDirection, 0.0f);

                //We instantiate each shot
                GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
                GameObject SecondShot = Instantiate(Bullet, transform.position, Quaternion.identity);
                GameObject ThirdShot = Instantiate(Bullet, transform.position, Quaternion.identity);

                //First Shot
                shot.GetComponent<IDamaging>().setDamage(Damage);
                shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;
                shot.transform.Rotate(0, Mathf.Atan2(direction.y, direction.x), 0);

                //Second Shot
                SecondShot.GetComponent<IDamaging>().setDamage(Damage);
                SecondShot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * SecondShotDir;
                SecondShot.transform.Rotate(0, Mathf.Atan2(SecondShotDir.y, SecondShotDir.x), 0);

                //Third Shot
                ThirdShot.GetComponent<IDamaging>().setDamage(Damage);
                ThirdShot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * ThirdShotDir;
                ThirdShot.transform.Rotate(0, Mathf.Atan2(ThirdShotDir.y, ThirdShotDir.x), 0);
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

