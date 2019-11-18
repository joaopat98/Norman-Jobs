using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D myRB;
    public float moveSpeed;
    public float stoppingDistance;

    public PlayerMovement thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        thePlayer = FindObjectOfType<PlayerMovement>();
    }
    private void FixedUpdate()
    {
       // myRB.velocity = transform.forward * moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, thePlayer.transform.position) > stoppingDistance) { 
        transform.position = Vector2.MoveTowards(transform.position, thePlayer.transform.position, moveSpeed * Time.deltaTime);
    }
    }
}
