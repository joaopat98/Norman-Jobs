using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    public Canvas Canvas;
    public GameObject Heart;
    public int MaxHP;
    public int HP;
    private List<GameObject> hearts;
    private Vector2 finalScale, leftTop;
    public CameraShake CameraShake;

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
        hearts = new List<GameObject>();
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
            StartCoroutine(Flashing());
            StartCoroutine(CameraShake.Shake(0.4f, 0.1f));
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
    }

    public void Hit(GameObject obj, float value)
    {
        HP = (int)Mathf.Round(Mathf.Max(HP - value, 0));
        if (HP == 0)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Destroy(hearts[i]);
            }
            hearts.Clear();
            Destroy(gameObject);
        }
    }

    private IEnumerator Flashing()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashingColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }
}
