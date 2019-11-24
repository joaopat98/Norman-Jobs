using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    
    // Start is called before the first frame update
    private float timeBtwPunchs;
    public float startTimeBtwPubchs;

    public GameObject Punch;
    public float MeleeSpeed = 1;
    public Sprite attackSprite;
    public Sprite normalSprite;
    public SpriteRenderer sprite;
    new void Start()
    {
        base.Start();
        timeBtwPunchs = startTimeBtwPubchs;
        normalSprite = GetComponent<SpriteRenderer>().sprite;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > stopDistance)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            this.GetComponent<Rigidbody2D>().velocity = speed * dir * Time.deltaTime;

        }
        else if (Vector2.Distance(player.transform.position, transform.position) < stopDistance)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            if (timeBtwPunchs <= 0)
            {
                Vector2 dir = (player.transform.position - transform.position).normalized;
               
                GameObject punch = Instantiate(Punch, transform.position, Quaternion.identity);

               

                punch.GetComponent<Rigidbody2D>().velocity = MeleeSpeed * dir;
                punch.transform.Rotate(0, Mathf.Atan2(dir.y, dir.x), 0);
                timeBtwPunchs = startTimeBtwPubchs;

                StartCoroutine(ChangeSprite());

            }
            else
            {
                timeBtwPunchs -= Time.deltaTime;
            }
        }


    }

    IEnumerator ChangeSprite()
    {
        sprite.sprite = attackSprite;
        yield return new WaitForSeconds(1);
        sprite.sprite = normalSprite;
    }
}
