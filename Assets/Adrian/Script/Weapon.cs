using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   
   
    // Start is called before the first frame update
    public float range = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
        if (Input.GetMouseButtonDown(0) && (thePlayer.transform.position - transform.position).magnitude < range)
        {      
            Debug.Log("Tenho arma");
            
            NormanAtribute playerScript = thePlayer.GetComponent<NormanAtribute>();
           
            playerScript.SetWeapon(Instantiate(this, thePlayer.transform.position, thePlayer.transform.rotation));
            Destroy(gameObject);
            
        }
    }

}
