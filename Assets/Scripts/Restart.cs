using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
   /* void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
