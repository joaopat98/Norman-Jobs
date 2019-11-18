using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormanAtribute : MonoBehaviour
{
    public Weapon weapon;

    void Update()
    {
        if(weapon != null)
        {
            GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
            weapon.transform.position = thePlayer.transform.position;
        }
    }
    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }


}
