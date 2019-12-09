using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public GameObject Reticle;
    public float Range = 1f;

    public Vector2 MousePos;
    private Weapon weapon = null;

    [Header("Sound Stuff")]
    public AudioClip weaponGrabSound;
    public float weaponGrabSoundVolume;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Reticle.transform.position = MousePos;

        if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && weapon != null && weapon.type == WeaponType.Ranged)
        {
            weapon.Shoot(transform.position, (MousePos - (Vector2)transform.position).normalized);
        }

        if (Input.GetButtonDown("Grab"))
        {
            Weapon switchWeapon = null;
            Weapon[] weapons = GameObject.FindGameObjectsWithTag("Weapon").Select(w => w.GetComponent<Weapon>()).ToArray();
            float minDist = Mathf.Infinity;
            foreach (var w in weapons)
            {
                float dist = (transform.position - w.transform.position).magnitude;
                if (weapon != w && dist < Range && dist < minDist)
                    switchWeapon = w;
            }
            if (switchWeapon)
            {
                SetWeapon(switchWeapon);
                AudioSource.PlayClipAtPoint(weaponGrabSound, Camera.main.transform.position, weaponGrabSoundVolume);
            }

        }
    }

    public void SetWeapon(Weapon newWeapon)
    {
        if (weapon)
        {
            weapon.Held = false;
            if (newWeapon)
            {
                newWeapon.Held = true;
                weapon.transform.position = newWeapon.transform.position;
            }
        }
        if (newWeapon)
        {
            newWeapon.Held = true;
        }
        weapon = newWeapon;
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }


}
