using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy
{

    // Start is called before the first frame update
    public float AttackRange;
    public float AttackTime;
    public float AttackDistance;
    public float BoxSize = 3.0f;
    bool IsChasing;
    public GameObject explosion;

    [Header("Sound Settings")]
    public float ExplosionSoundVolume;
    public AudioClip ExplosionSound;
    public AudioClip BeepSound;

    bool stop = true;

    new void Start()
    {
        base.Start();
        IsChasing = false;
       
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        
    }

    protected override void Act()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        Vector2 dir = (player.transform.position - transform.position).normalized;

        Vector2Int dir_ceil = dir.ToSpriteDirection(0.2f);

        if (dir.x > 0)
        {
            var scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (player != null && dist > stopDistance)
        {
            rb.MovePosition(rb.position + (dir * speed * Time.fixedDeltaTime));
            animator.SetInteger("x", dir_ceil.x);
            animator.SetInteger("y", dir_ceil.y);
            IsChasing = true;
            
        }
        
        if (IsChasing)
        {
            if (stop)
            {
                stop = false;
                StartCoroutine(activeExplosion());
                StartCoroutine(ExplosionTimer());
                
            }
            
        }
    }

    protected override void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    private IEnumerator ExplosionTimer()
    {
        int temp = 0;
        float yellowTime = 1.0f;
        float deltaTime = 0.2f;
        
        while (temp < 4 && isAlive())
        {

            yellowTime -= deltaTime;
            
            spr.color = new Color(255, 165, 0, 1);
            yield return new WaitForSeconds(yellowTime);
            spr.color = oldColor;
            yield return new WaitForSeconds(yellowTime);
            temp++;

        }
        yield return new WaitForSeconds(0.50f);
        if (isAlive())
        {
            TryExplode();
            Destroy(gameObject);
        }
    }

    public void TryExplode()
    {
        AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position, ExplosionSoundVolume);
        var ExplosionInstance = Instantiate(explosion, transform.position,Quaternion.identity);
    }

    private IEnumerator activeExplosion()
    {

        int i = 0;
        while (i < 5 && isAlive())
        {
            yield return new WaitForSeconds(0.9f);
            AudioSource.PlayClipAtPoint(BeepSound, this.transform.position, ExplosionSoundVolume);
            i++;
        }

        
    }

}
