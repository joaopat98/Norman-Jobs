using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletDisplay : MonoBehaviour
{
    public GameObject player;
    Weapon weapon;
    private Text bulletText;
    private bool hasWeapon;
    void Start()
    {
        hasWeapon = false;
        bulletText = GetComponent<Text>();
        bulletText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

        if (player)
        {
            weapon = player.GetComponent<MouseMovement>().GetWeapon();
            if (weapon != null)
            {
                hasWeapon = true;
                bulletText.text = weapon.Ammo.ToString();

            }
            else if (hasWeapon && weapon == null)
            {
                hasWeapon = false;
                bulletText.text = "0";
            }
        }
    }
}
