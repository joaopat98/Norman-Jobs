using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    public Canvas Canvas;
    public GameObject Heart;
    public int MaxHP;
    public int HP;
    public GameObject panel;

    public GameObject player;

    public bool alive = true;
    private List<GameObject> hearts;
    private Vector2 finalScale, leftTop;
    public CameraShake CameraShake;
    public bool hurting;

    private bool invincible;

    private Animator animator;
    private Rigidbody2D rb;

    private Punch punch;

    public float HitDistance;

    public float HitTime;

    [Header("Sound Stuff")]
    public AudioClip deathSound;
    public float deathSoundVolume;
    public AudioClip loseSound;
    public float loseSoundVolume;
    public AudioClip HurtSound;
    public float hurtSoundVolume;

    [Header("Invulnerability Stuff")]
    public Color flashingColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        punch = GetComponent<Punch>();

        hearts = new List<GameObject>();
        panel.SetActive(false);
        HP = MaxHP;
        var rect = Canvas.pixelRect;
        leftTop = new Vector2(rect.xMin, rect.yMax);
        Vector2 sizeDelta = Heart.GetComponent<RectTransform>().sizeDelta;
        Vector2 canvasScale = new Vector2(Canvas.transform.localScale.x, Canvas.transform.localScale.y);

        finalScale = new Vector2(sizeDelta.x * canvasScale.x, sizeDelta.y * canvasScale.y);
        for (int i = 0; i < MaxHP; i++)
        {
            hearts.Add(Instantiate(Heart, leftTop + new Vector2(finalScale.x / 2, -finalScale.y / 2) + new Vector2(i * finalScale.x, 0), Quaternion.identity, Canvas.transform));
        }



    }

    // Update is called once per frame
    void Update()
    {
        int dif = HP - hearts.Count;
        if (dif < 0)
        {
            for (int i = 0; i > dif; i--)
            {
                Destroy(hearts[hearts.Count - 1]);
                hearts.RemoveAt(hearts.Count - 1);
            }

        }
        else if (dif > 0)
        {
            for (int i = 0; i < dif; i++)
            {
                hearts.Add(Instantiate(Heart, leftTop + new Vector2(finalScale.x / 2, -finalScale.y / 2) + new Vector2(hearts.Count * finalScale.x, 0), Quaternion.identity, Canvas.transform));
            }
        }

        player.GetComponent<ScoreScriptPlayer>().GetHPScore(HP);
    }

    public void Hit(GameObject obj, float value)
    {


        HP = (int)Mathf.Round(Mathf.Max(HP - value, 0));
        AudioSource.PlayClipAtPoint(HurtSound, Camera.main.transform.position, hurtSoundVolume);

        if (HP == 0)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Destroy(hearts[i]);
            }
            hearts.Clear();
            alive = false;
            animator.SetBool("alive", false);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
            StartCoroutine(LoseSound());
        }
        else
        {
            if (!hurting)
            {
                hurting = true;
                GetComponent<Punch>().punching = false;
                var lookAt = ((Vector2)obj.transform.position - (Vector2)transform.position).ToSpriteDirection(0.2f);

                ////BUG DE FICAR PARADO
                if (lookAt.x == 0 && lookAt.y == 0)
                {
                    lookAt.x = 1;
                }
                ////-------

                if (lookAt.x > 0)
                {
                    var scale = transform.localScale;
                    scale.x = -1;
                    transform.localScale = scale;
                }
                else
                {
                    var scale = transform.localScale;
                    scale.x = 1;
                    transform.localScale = scale;
                }

                /* Debug.Log("X: " + lookAt.x);
                 Debug.Log("Y: " + lookAt.y);*/

                animator.SetInteger("x", lookAt.x);
                animator.SetInteger("y", lookAt.y);
                StartCoroutine(CameraShake.Shake(0.4f, 0.1f));
                invincible = true;
                StartCoroutine(Flashing());
                StartCoroutine(PushBack(transform.position - obj.transform.position, value));
                animator.SetTrigger("hurt");
            }
        }
    }

    public void Hit(GameObject obj, float value, float knockback)
    {
        HP = (int)Mathf.Round(Mathf.Max(HP - value, 0));

        if (HP == 0)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Destroy(hearts[i]);
            }
            hearts.Clear();
            alive = false;
            animator.SetBool("alive", false);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
            StartCoroutine(LoseSound());
        }
        else
        {
            if (!hurting)
            {
                hurting = true;
                GetComponent<Punch>().punching = false;
                var lookAt = ((Vector2)obj.transform.position - (Vector2)transform.position).ToSpriteDirection(0.2f);

                ////BUG DE FICAR PARADO
                if (lookAt.x == 0 && lookAt.y == 0)
                {
                    lookAt.x = 1;
                }
                ////-------

                if (lookAt.x > 0)
                {
                    var scale = transform.localScale;
                    scale.x = -1;
                    transform.localScale = scale;
                }
                else
                {
                    var scale = transform.localScale;
                    scale.x = 1;
                    transform.localScale = scale;
                }

                /* Debug.Log("X: " + lookAt.x);
                 Debug.Log("Y: " + lookAt.y);*/

                animator.SetInteger("x", lookAt.x);
                animator.SetInteger("y", lookAt.y);
                StartCoroutine(CameraShake.Shake(0.4f, 0.1f));
                invincible = true;
                StartCoroutine(Flashing());
                StartCoroutine(PushBack(transform.position - obj.transform.position, knockback));
                animator.SetTrigger("hurt");
            }
        }
    }

    IEnumerator PushBack(Vector3 direction, float damage)
    {
        float curTime = 0;
        Vector2 finalPos = transform.position + (direction.normalized * HitDistance * damage);
        Vector2 origPos = transform.position;
        float startTime = Time.time;
        while (curTime < HitTime)
        {
            rb.MovePosition(Vector2.Lerp(origPos, finalPos, EasingFunction.EaseOutCubic(0, 1, curTime / HitTime)));
            yield return new WaitForFixedUpdate();
            curTime = Time.time - startTime;
        }
    }

    public void FinishHurting()
    {
        punch.punching = false;
        hurting = false;
    }

    private IEnumerator Flashing()
    {
        int temp = 0;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashingColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        yield return new WaitForSeconds(0.70f);
        invincible = false;

    }

    public bool isAlive()
    {
        return alive;
    }

    IEnumerator LoseSound()
    {
        yield return new WaitForSeconds(2.0f);
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, loseSoundVolume);
        yield return new WaitForSeconds(2.0f);

        panel.SetActive(true);
    }
}
