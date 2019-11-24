using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    public float timeBtwPunches;
    public float startTimeBtwPunches;
    public float punchDuration;
    public float attackDistance;
    public float Damage;
    public float BoxSize;
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(col.gameObject);
        }
    }

    void Update()
    {

        if (timeBtwPunches <= 0 && Input.GetButtonDown("Fire2"))
        {
            timeBtwPunches = startTimeBtwPunches;
            Vector2 dir = GetComponent<PlayerMovement>().lookingAt;
            var hit = Physics2D.BoxCast(transform.position, Vector2.one * BoxSize, Mathf.Atan2(dir.x, dir.y), dir, attackDistance, LayerMask.GetMask("Enemies"));
            if (hit.collider != null)
            {
                hit.transform.GetComponent<Enemy>().Hit(null, Damage);
            }
        }
        else
        {
            timeBtwPunches -= Time.deltaTime;
        }
    }

}
