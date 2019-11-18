using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public GameObject Reticle;
    public GameObject Bullet;
    public float BulletSpeed = 1;
    public float ShootInterval;

    private float shootTimer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Reticle.transform.position = mousePos;

        if (shootTimer <= 0 && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
        {
            Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
            GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = BulletSpeed * dir;
            shot.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
            shootTimer = ShootInterval;
        }
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }
}
