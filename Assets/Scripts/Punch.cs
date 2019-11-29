using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    public float timeBtwPunches;
    public float startTimeBtwPunches;
    public float attackDistance;
    public float Damage;
    public float BoxSize;
    public bool punching = false;

    private Animator anim;
    private PlayerMovement movement;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector2 dir = movement.lookingAt;
        Debug.DrawRay(transform.position, dir * attackDistance, Color.green, 0.016f);

        if (timeBtwPunches <= 0 && Input.GetButtonDown("Fire2"))
        {
            punching = true;
            anim.SetTrigger("punch");
            timeBtwPunches = startTimeBtwPunches;
        }
        else
        {
            timeBtwPunches -= Time.deltaTime;
        }
    }

    public void TryPunch()
    {
        Vector2 dir = movement.lookingAt;
        var hit = Physics2D.BoxCast(transform.position, Vector2.one * BoxSize, 0, dir, attackDistance, LayerMask.GetMask("Enemies"));
        if (hit.collider != null)
        {
            hit.transform.GetComponent<Enemy>().Hit(null, Damage);
        }
    }

    public void FinishPunch()
    {
        punching = false;
    }
}
