using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var sprite in GetSprites())
        {
            sprite.sortingOrder = 10000 - Mathf.RoundToInt(sprite.bounds.min.y * 100f);
        }
    }

    private List<SpriteRenderer> GetSprites()
    {
        var sprites = new List<SpriteRenderer>();
        var allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        foreach (var sprite in allSprites)
        {
            if (sprite.gameObject.GetComponent<RectTransform>() == null && !sprite.CompareTag("Ground"))
                sprites.Add(sprite);
        }
        return sprites;
    }
}
