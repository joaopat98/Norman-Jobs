﻿using System.Collections;
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
    public float range = 1.0f;
    public WeaponType type;

    public GameObject Bullet;
    public float BulletSpeed = 1;
    public float ShootInterval;

    private float shootTimer = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
        var playerScript = thePlayer.GetComponent<MouseMovement>();
        if (Input.GetButtonDown("Grab") && (thePlayer.transform.position - transform.position).magnitude < range
                && playerScript.GetWeapon() != this)
        {
            Debug.Log("Tenho arma");
            transform.SetParent(thePlayer.transform);
            transform.position = thePlayer.transform.position;
            playerScript.SetWeapon(this);
        }
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        if (shootTimer <= 0)
        {
            GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * direction;
            shot.transform.Rotate(0, Mathf.Atan2(direction.y, direction.x), 0);
            shootTimer = ShootInterval;
        }
    }

}
