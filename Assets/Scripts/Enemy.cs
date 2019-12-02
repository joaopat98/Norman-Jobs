using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHealthSystem
{

    public float speed;
    public float stopDistance;
    protected GameObject player;

    public GameObject WeaponDrop;

    public float Radar;

    public float HP;

    private SpriteRenderer spr;
    private Color oldColor;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spr = GetComponent<SpriteRenderer>();
        oldColor = spr.color;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist < Radar)
            Act();
        else
        {
            Idle();
        }
    }
    public virtual void Hit(GameObject obj, float value)
    {
        HP -= value;
        StartCoroutine(Tint());
        if (HP <= 0)
        {
            if (WeaponDrop)
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    protected IEnumerator Tint()
    {
        spr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spr.color = oldColor;
    }

    protected abstract void Act();
    protected abstract void Idle();
}



