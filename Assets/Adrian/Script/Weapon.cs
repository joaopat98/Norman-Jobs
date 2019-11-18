using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    // Start is called before the first frame update
    public float range = 1.0f;
    private bool held = false;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
        if (!held && Input.GetButtonDown("Grab") && (thePlayer.transform.position - transform.position).magnitude < range)
        {
            Debug.Log("Tenho arma");
            transform.SetParent(thePlayer.transform);
            transform.position = thePlayer.transform.position;
            held = true;
        }
    }

}
