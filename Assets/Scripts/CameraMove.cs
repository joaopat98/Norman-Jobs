﻿using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            var cameraPos = transform.position;
            var playerPos = player.position;
            cameraPos.x = Mathf.Min(3, Mathf.Max(-4f, playerPos.x));
            cameraPos.y = playerPos.y;
            transform.position = cameraPos;
        }
    }
}
