using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    private bool lockDoor;
    public GameObject door;

    void Start()
    {
        lockDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0 && lockDoor)
        {
            lockDoor = false;
            door.SetActive(false);
        }
    }

}
