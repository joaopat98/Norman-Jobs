using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(col.gameObject);
        }
    }
}
